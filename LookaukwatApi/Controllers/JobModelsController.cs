using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using LookaukwatApi.Models;
using LookaukwatApi.Services;
using LookaukwatApi.ViewModel;
using Microsoft.AspNet.Identity;

namespace LookaukwatApi.Controllers
{
    public class JobModelsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/JobModels
        public List<JobModel> GetProductModels()
        {
            return db.Jobs.ToList();
        }

        // GET: api/JobModels/5
        [ResponseType(typeof(JobViewModel))]
        public async Task<IHttpActionResult> GetJobModel(int id)
        {
            JobModel jobModel = await db.Jobs.FindAsync(id);
            if (jobModel == null)
            {
                return NotFound();
            }

            jobModel.ViewNumber++;
            await db.SaveChangesAsync();

            var ListeSimilar = db.Jobs.Where(m => m.Category.CategoryName == jobModel.Category.CategoryName &&
            m.Town == jobModel.Town && m.SearchOrAskJob == jobModel.SearchOrAskJob && m.id != jobModel.id).OrderBy(o => Guid.NewGuid()).
            Take(6).Select(s => new SimilarProductViewModel
            {
                id = s.id,
                Category = s.Category.CategoryName,
                Title = s.Title,
                Price = s.Price,
                Town = s.Town,
                DateAdd = s.DateAdd,
                Image = s.Images.Select(m => m.ImageMobile).FirstOrDefault(),

            }).ToList();

            List<SimilarProductViewModel> Liste = new List<SimilarProductViewModel>();

            foreach (var item in ListeSimilar)
            {
                item.Date = ConvertDate.Convert(item.DateAdd);
                Liste.Add(item);
            }


            JobViewModel job = new JobViewModel 
            {
                id = jobModel.id,
                Title = jobModel.Title,
                Description = jobModel.Description,
                DateAdd = jobModel.DateAdd,
                Date = ConvertDate.Convert(jobModel.DateAdd),
                Images = jobModel.Images,
                User = jobModel.User,
                Town = jobModel.Town,
                ActivitySector = jobModel.ActivitySector,
            TypeContract = jobModel.TypeContract,
            SearchOrAskJob = jobModel.SearchOrAskJob,
            Price = jobModel.Price,
            Street = jobModel.Street,
            SimilarProduct = Liste,
            ViewNumber = jobModel.ViewNumber,
                Lat = jobModel.Coordinate.Lat,
                Lon = jobModel.Coordinate.Lon,
            };

            return Ok(job);
        }

        // PUT: api/JobModels/5
        [ResponseType(typeof(void))]
            [Authorize]
        public async Task<IHttpActionResult> PutJobModel(int id, JobModel jobModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != jobModel.id)
            {
                return BadRequest();
            }

            db.Entry(jobModel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // Result of Offer search Job
        [Route("api/JobModels/GetOfferJobSearchNumber")]
        public async Task<int> GetOfferJobSearchNumber(string categori, string town, string searchOrAskJob, string typeContract, string activitySector, int price)
        {

            var results = await db.Jobs.
              Select(s => new SearchViewModel
              {
                  Category = s.Category.CategoryName,
                  Town = s.Town,
                  SearchOrAskJob = s.SearchOrAskJob,
                  Price = s.Price,
                  TypeContract = s.TypeContract,
                  ActivitySector = s.ActivitySector
                  
              }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob ).ToListAsync();

            if (price >= 0 && price < 300000)
            {
                results = results.Where(m => m.Price <= price).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town) && town != "Toutes")
            {
                results = results.Where(m => m.Town == town).ToList();
            }

            if (!string.IsNullOrWhiteSpace(typeContract) && typeContract != "Tout")
            {
                results = results.Where(m => m.TypeContract == typeContract).ToList();
            }

            if (!string.IsNullOrWhiteSpace(activitySector) && activitySector != "Tout")
            {
                results = results.Where(m => m.ActivitySector == activitySector).ToList();
            }


            return results.Count;
        }

        // Result of Offer search Job
        [Route("api/JobModels/GetOfferJobSearch")]
        public async Task<List<ProductForMobile>> GetOfferJobSearch(string categori, string town, string searchOrAskJob, string typeContract, string activitySector, int price, int pageIndex, int pageSize, string sortBy)
        {
            List<SearchViewModel> results = new List<SearchViewModel>();
            switch (sortBy)
            {
                case "MostRecent":
                    results = await db.Jobs.OrderByDescending(o => o.id).
              Select(s => new SearchViewModel
              {
                  id = s.id,
                  Title = s.Title,
                  Images = s.Images,
                  ViewNumber = s.ViewNumber,
                  DateAdd = s.DateAdd,
                  Category = s.Category.CategoryName,
                  Town = s.Town,
                  SearchOrAskJob = s.SearchOrAskJob,
                  Price = s.Price,
                  TypeContract = s.TypeContract,
                  ActivitySector = s.ActivitySector

              }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();

                    break;
                case "MostOld":
                    results = await db.Jobs.OrderBy(o => o.id).
              Select(s => new SearchViewModel
              {
                  id = s.id,
                  Title = s.Title,
                  Images = s.Images,
                  ViewNumber = s.ViewNumber,
                  DateAdd = s.DateAdd,
                  Category = s.Category.CategoryName,
                  Town = s.Town,
                  SearchOrAskJob = s.SearchOrAskJob,
                  Price = s.Price,
                  TypeContract = s.TypeContract,
                  ActivitySector = s.ActivitySector

              }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                case "LowerPrice":
                    results = await db.Jobs.OrderBy(o => o.Price).
              Select(s => new SearchViewModel
              {
                  id = s.id,
                  Title = s.Title,
                  Images = s.Images,
                  ViewNumber = s.ViewNumber,
                  DateAdd = s.DateAdd,
                  Category = s.Category.CategoryName,
                  Town = s.Town,
                  SearchOrAskJob = s.SearchOrAskJob,
                  Price = s.Price,
                  TypeContract = s.TypeContract,
                  ActivitySector = s.ActivitySector

              }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                case "HeigherPrice":
                    results = await db.Jobs.OrderByDescending(o => o.Price).
              Select(s => new SearchViewModel
              {
                  id = s.id,
                  Title = s.Title,
                  Images = s.Images,
                  ViewNumber = s.ViewNumber,
                  DateAdd = s.DateAdd,
                  Category = s.Category.CategoryName,
                  Town = s.Town,
                  SearchOrAskJob = s.SearchOrAskJob,
                  Price = s.Price,
                  TypeContract = s.TypeContract,
                  ActivitySector = s.ActivitySector

              }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                default:
                    results = await db.Jobs.OrderByDescending(o=>o.id).
              Select(s => new SearchViewModel
              {
                  id = s.id,
                  Title = s.Title,
                  Images = s.Images,
                  ViewNumber = s.ViewNumber,
                  DateAdd = s.DateAdd,
                  Category = s.Category.CategoryName,
                  Town = s.Town,
                  SearchOrAskJob = s.SearchOrAskJob,
                  Price = s.Price,
                  TypeContract = s.TypeContract,
                  ActivitySector = s.ActivitySector

              }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();

                    break;
            }


             
            if (price >= 0)
            {
                results = results.Where(m => m.Price <= price).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town) && town != "Toutes")
            {
                results = results.Where(m => m.Town == town).ToList();
            }

            if (!string.IsNullOrWhiteSpace(typeContract) && typeContract != "Tout")
            {
                results = results.Where(m => m.TypeContract == typeContract).ToList();
            }

            if (!string.IsNullOrWhiteSpace(activitySector) && activitySector != "Tout")
            {
                results = results.Where(m => m.ActivitySector == activitySector).ToList();
            }


            var List = results.Skip(pageIndex * pageSize).Take(pageSize).Select(s => new ProductForMobile
            {
                Title = s.Title,
                Town = s.Town,
                DateAdd = s.DateAdd,
                Category = s.Category,
                Image = s.Images.Select(im => im.ImageMobile).FirstOrDefault(),
                id = s.id,
                Price = s.Price,
                ViewNumber = s.ViewNumber
            }).ToList();

            List<ProductForMobile> Liste = new List<ProductForMobile>();

            foreach (var item in List)
            {
                item.Date = ConvertDate.Convert(item.DateAdd);
                Liste.Add(item);
            }


            return Liste;
        }


        // POST: api/JobModels
        [ResponseType(typeof(JobModel))]
        [Authorize]
        public async Task<IHttpActionResult> PostJobModel(JobModel jobModel)
        {
            //jobModel.DateAdd = DateTime.Now;
            //jobModel.Category = new CategoryModel() { CategoryName = "Emploi" };
            //jobModel.Coordinate = new ProductCoordinateModel() { Lat = "1.25", Lon = "2.1265" };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string UserId = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(m=>m.Id == UserId);
           List<ImageProcductModel> img= new List<ImageProcductModel>();
            var im = new ImageProcductModel() { id = Guid.NewGuid(), Image = "https://particulier-employeur.fr/wp-content/themes/fepem/img/general/avatar.png",ImageMobile = "https://particulier-employeur.fr/wp-content/themes/fepem/img/general/avatar.png" };
            img.Add(im);
           jobModel.User = user;
            jobModel.DateAdd = DateTime.UtcNow;
            if (jobModel.Coordinate == null || (jobModel.Coordinate != null &&
                (jobModel.Coordinate.Lat == null || jobModel.Coordinate.Lon == null)))
            {
                jobModel.Coordinate = await CoordonateService.GetCoodinateAsync(jobModel.Town, jobModel.Street);
            }

            jobModel.Images= img;


            db.Jobs.Add(jobModel);
           
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = jobModel.id }, jobModel);
        }

        // DELETE: api/JobModels/5
        [ResponseType(typeof(JobModel))]
        public async Task<IHttpActionResult> DeleteJobModel(int id)
        {
            JobModel jobModel = await db.Jobs.FindAsync(id);
            if (jobModel == null)
            {
                return NotFound();
            }

            db.Jobs.Remove(jobModel);
            await db.SaveChangesAsync();

            return Ok(jobModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JobModelExists(int id)
        {
            return db.Jobs.Count(e => e.id == id) > 0;
        }
    }
}