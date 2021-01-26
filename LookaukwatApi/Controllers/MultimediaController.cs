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
    public class MultimediaController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Multimedia
        public IQueryable<MultimediaModel> GetProducts()
        {
            return db.Multimedia;
        }

        // GET: api/Multimedia/5
        [ResponseType(typeof(MultimediaViewModel))]
        public async Task<IHttpActionResult> GetMultimediaModel(int id)
        {
            MultimediaModel multimediaModel = await db.Multimedia.FindAsync(id);
            if (multimediaModel == null)
            {
                return NotFound();
            }
            multimediaModel.ViewNumber++;
            await db.SaveChangesAsync();

            var ListeSimilar = db.Multimedia.Where(m =>
           m.Town == multimediaModel.Town && m.SearchOrAskJob == multimediaModel.SearchOrAskJob && m.id != multimediaModel.id && m.Type == multimediaModel.Type).OrderBy(o => Guid.NewGuid()).
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
            Liste = ListeSimilar;
            //foreach (var item in ListeSimilar)
            //{
            //    item.Date = ConvertDate.Convert(item.DateAdd);
            //    Liste.Add(item);
            //}


            MultimediaViewModel multimedia = new MultimediaViewModel
            {
                id = multimediaModel.id,
                Title = multimediaModel.Title,
                Description = multimediaModel.Description,
                DateAdd = multimediaModel.DateAdd,
                Date = ConvertDate.Convert(multimediaModel.DateAdd),
                Images = multimediaModel.Images.Select(s => s.ImageMobile).ToList(),
                UserName = multimediaModel.User.FirstName,
                UserEmail = multimediaModel.User.Email,
                UserPhone = multimediaModel.User.PhoneNumber,
                Town = multimediaModel.Town,
                Brand = multimediaModel.Brand,
                Type = multimediaModel.Type,
                SearchOrAsk = multimediaModel.SearchOrAskJob,
                Model = multimediaModel.Model,
                Price = multimediaModel.Price,
                Capacity = multimediaModel.Capacity,
                Street = multimediaModel.Street,
                SimilarProduct = Liste,
                Lat = multimediaModel.Coordinate.Lat,
                Lon = multimediaModel.Coordinate.Lon,
                ViewNumber = multimediaModel.ViewNumber
            };

            return Ok(multimedia);
        }

        [HttpGet]
        [Route("api/Multimedia/GetMultimediaCritere")]
        [ResponseType(typeof(MultimediaViewModel))]
        public async Task<IHttpActionResult> GetMultimediaCritere(int id)
        {
            MultimediaModel Mode = await db.Multimedia.FindAsync(id);
            if (Mode == null)
            {
                return NotFound();
            }

            MultimediaViewModel multi = new MultimediaViewModel
            {
                id = Mode.id,
                Price = Mode.Price,
                SearchOrAsk = Mode.SearchOrAskJob,
                 Type = Mode.Type,
                Brand = Mode.Brand,
                Model = Mode.Model,
                Capacity = Mode.Capacity,
                
            };
            return Ok(multi);
        }

        // Result of Offer search Multimedia
        [Route("api/Multimedia/GetOfferMultiSearchNumber")]
        public async Task<int> GetOfferMultiSearchNumber(string categori, string town, string searchOrAskJob, int price, string multimediaRubrique, string multimediaBrand, string multimediaModel, string multimediaCapacity)
        {

            var results = await db.Multimedia.
              Select(s => new SearchViewModel
              {
                  Category = s.Category.CategoryName,
                  Town = s.Town,
                  SearchOrAskJob = s.SearchOrAskJob,
                  Price = s.Price,
                  MultimediaBrand = s.Brand,
                  MultimediaModel = s.Model,
                  MultimediaRubrique = s.Type,
                  MultimediaCapacity = s.Capacity

              }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();

            if (price >= 0 && price < 100000)
            {
                results = results.Where(m => m.Price <= price).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town) && town != "Toutes")
            {
                results = results.Where(m => m.Town == town).ToList();
            }

            if (!string.IsNullOrWhiteSpace(multimediaBrand) && multimediaBrand != "Toutes")
            {
                results = results.Where(m => m.MultimediaBrand != null && m.MultimediaBrand == multimediaBrand).ToList();
            }

            if (!string.IsNullOrWhiteSpace(multimediaModel) && multimediaModel != "Tout")
            {
                results = results.Where(m => m.MultimediaModel != null && m.MultimediaModel == multimediaModel).ToList();
            }

            if (!string.IsNullOrWhiteSpace(multimediaRubrique) && multimediaRubrique != "Toutes")
            {
                results = results.Where(m => m.MultimediaRubrique != null && m.MultimediaRubrique == multimediaRubrique).ToList();
            }

            if (!string.IsNullOrWhiteSpace(multimediaCapacity) && multimediaCapacity != "Toutes")
            {
                results = results.Where(m => m.MultimediaCapacity != null && m.MultimediaCapacity == multimediaCapacity).ToList();
            }

            return results.Count;
        }

        // Result of Offer search Multimedia
        [Route("api/Multimedia/GetOfferMultiSearch")]
        public async Task<List<ProductForMobile>> GetOfferMultiSearch(string categori, string town, string searchOrAskJob, int price, string multimediaRubrique, string multimediaBrand, string multimediaModel, string multimediaCapacity, int pageIndex, int pageSize, string sortBy)
        {
            List<SearchViewModel> results = new List<SearchViewModel>();

            switch (sortBy)
            {
                case "MostRecent":
                    results = await db.Multimedia.OrderByDescending(o => o.id).
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
                MultimediaBrand = s.Brand,
                MultimediaModel = s.Model,
                MultimediaRubrique = s.Type,
                MultimediaCapacity = s.Capacity

            }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                case "MostOld":
                    results = await db.Multimedia.OrderBy(o => o.id).
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
                MultimediaBrand = s.Brand,
                MultimediaModel = s.Model,
                MultimediaRubrique = s.Type,
                MultimediaCapacity = s.Capacity

            }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                case "LowerPrice":
                    results = await db.Multimedia.OrderBy(o => o.Price).
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
                MultimediaBrand = s.Brand,
                MultimediaModel = s.Model,
                MultimediaRubrique = s.Type,
                MultimediaCapacity = s.Capacity

            }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                case "HeigherPrice":
                    results = await db.Multimedia.OrderByDescending(o => o.Price).
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
                MultimediaBrand = s.Brand,
                MultimediaModel = s.Model,
                MultimediaRubrique = s.Type,
                MultimediaCapacity = s.Capacity

            }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                default:

                    results = await db.Multimedia.OrderByDescending(o => o.id).
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
                 MultimediaBrand = s.Brand,
                 MultimediaModel = s.Model,
                 MultimediaRubrique = s.Type,
                 MultimediaCapacity = s.Capacity

             }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();

                    break;
            }

            

            if (price >= 0 && price < 100000)
            {
                results = results.Where(m => m.Price <= price).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town) && town != "Toutes")
            {
                results = results.Where(m => m.Town == town).ToList();
            }

            if (!string.IsNullOrWhiteSpace(multimediaBrand) && multimediaBrand != "Toutes")
            {
                results = results.Where(m => m.MultimediaBrand != null && m.MultimediaBrand == multimediaBrand).ToList();
            }

            if (!string.IsNullOrWhiteSpace(multimediaModel) && multimediaModel != "Tout")
            {
                results = results.Where(m => m.MultimediaModel != null && m.MultimediaModel == multimediaModel).ToList();
            }

            if (!string.IsNullOrWhiteSpace(multimediaRubrique) && multimediaRubrique != "Toutes")
            {
                results = results.Where(m => m.MultimediaRubrique != null && m.MultimediaRubrique == multimediaRubrique).ToList();
            }

            if (!string.IsNullOrWhiteSpace(multimediaCapacity) && multimediaCapacity != "Toutes")
            {
                results = results.Where(m => m.MultimediaCapacity != null && m.MultimediaCapacity == multimediaCapacity).ToList();
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

            List<SearchViewModel> Liste = new List<SearchViewModel>();

            Liste = results.ToList();
            var Lis = Liste.Skip(pageIndex * pageSize).Take(pageSize).Select(s => new ProductForMobile
            {
                Title = s.Title,
                Town = s.Town,
                DateAdd = s.DateAdd,
                Category = s.Category,
                Image = s.Images.Select(im => im.ImageMobile).FirstOrDefault(),
                id = s.id,
                Price = s.Price,
                Date = s.Date,
                ViewNumber = s.ViewNumber
            }).ToList();


            return Lis;
        }

        // PUT: api/Multimedia/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMultimediaModel(int id, MultimediaCritereViewModel multimediaModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != multimediaModel.id)
            {
                return BadRequest();
            }
            var multi = await db.Multimedia.FirstOrDefaultAsync(m => m.id == id);


            multi.Price = multimediaModel.Price;
            multi.SearchOrAskJob = multimediaModel.SearchOrAsk;
            multi.Type = multimediaModel.Type;
            multi.Brand = multimediaModel.Brand;
            multi.Capacity = multimediaModel.Capacity;
            multi.Model = multimediaModel.Model;
           


            db.Entry(multi).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MultimediaModelExists(id))
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

        // POST: api/Multimedia
        [ResponseType(typeof(MultimediaModel))]
        public async Task<IHttpActionResult> PostMultimediaModel(MultimediaModel multimediaModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string UserId = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(m => m.Id == UserId);
            List<ImageProcductModel> img = new List<ImageProcductModel>();
            var im = new ImageProcductModel() { id = Guid.NewGuid(), Image = "https://particulier-employeur.fr/wp-content/themes/fepem/img/general/avatar.png", ImageMobile = "https://particulier-employeur.fr/wp-content/themes/fepem/img/general/avatar.png" };
            img.Add(im);
            multimediaModel.User = user;
            multimediaModel.DateAdd = DateTime.UtcNow;
            if (multimediaModel.Coordinate == null || (multimediaModel.Coordinate != null &&
               (multimediaModel.Coordinate.Lat == null || multimediaModel.Coordinate.Lon == null)))
            {
                multimediaModel.Coordinate = await CoordonateService.GetCoodinateAsync(multimediaModel.Town, multimediaModel.Street);
            }

            multimediaModel.Images = img;

            db.Multimedia.Add(multimediaModel);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = multimediaModel.id }, multimediaModel);
        }

        // DELETE: api/Multimedia/5
        [ResponseType(typeof(MultimediaModel))]
        public async Task<IHttpActionResult> DeleteMultimediaModel(int id)
        {
            MultimediaModel multimediaModel = await db.Multimedia.FindAsync(id);
            if (multimediaModel == null)
            {
                return NotFound();
            }

            db.Products.Remove(multimediaModel);
            await db.SaveChangesAsync();

            return Ok(multimediaModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MultimediaModelExists(int id)
        {
            return db.Products.Count(e => e.id == id) > 0;
        }
    }
}