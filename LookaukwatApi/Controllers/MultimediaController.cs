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

            var ListeSimilar = db.Multimedia.Where(m => m.Category.CategoryName == multimediaModel.Category.CategoryName &&
           m.Town == multimediaModel.Town && m.SearchOrAskJob == multimediaModel.SearchOrAskJob &&
           m.SearchOrAskJob == multimediaModel.SearchOrAskJob && m.id != multimediaModel.id && m.Type == multimediaModel.Type).OrderBy(o => Guid.NewGuid()).
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


            MultimediaViewModel multimedia = new MultimediaViewModel
            {
                id = multimediaModel.id,
                Title = multimediaModel.Title,
                Description = multimediaModel.Description,
                DateAdd = multimediaModel.DateAdd,
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
                SimilarProduct = ListeSimilar,
                ViewNumber = multimediaModel.ViewNumber
            };

            return Ok(multimedia);
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

            if (price >= 0)
            {
                results = results.Where(m => m.Price <= price).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town))
            {
                results = results.Where(m => m.Town == town).ToList();
            }

            if (!string.IsNullOrWhiteSpace(multimediaBrand))
            {
                results = results.Where(m => m.MultimediaBrand == multimediaBrand).ToList();
            }

            if (!string.IsNullOrWhiteSpace(multimediaModel))
            {
                results = results.Where(m => m.MultimediaModel == multimediaModel).ToList();
            }

            if (!string.IsNullOrWhiteSpace(multimediaRubrique))
            {
                results = results.Where(m => m.MultimediaRubrique == multimediaRubrique).ToList();
            }

            if (!string.IsNullOrWhiteSpace(multimediaCapacity))
            {
                results = results.Where(m => m.MultimediaCapacity == multimediaCapacity).ToList();
            }

            return results.Count;
        }

        // Result of Offer search Multimedia
        [Route("api/Multimedia/GetOfferMultiSearchNumber")]
        public async Task<List<ProductForMobile>> GetOfferMultiSearchNumber(string categori, string town, string searchOrAskJob, int price, string multimediaRubrique, string multimediaBrand, string multimediaModel, string multimediaCapacity, int pageIndex, int pageSize)
        {

            var results = await db.Multimedia.
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

            if (price >= 0)
            {
                results = results.Where(m => m.Price <= price).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town))
            {
                results = results.Where(m => m.Town == town).ToList();
            }

            if (!string.IsNullOrWhiteSpace(multimediaBrand))
            {
                results = results.Where(m => m.MultimediaBrand == multimediaBrand).ToList();
            }

            if (!string.IsNullOrWhiteSpace(multimediaModel))
            {
                results = results.Where(m => m.MultimediaModel == multimediaModel).ToList();
            }

            if (!string.IsNullOrWhiteSpace(multimediaRubrique))
            {
                results = results.Where(m => m.MultimediaRubrique == multimediaRubrique).ToList();
            }

            if (!string.IsNullOrWhiteSpace(multimediaCapacity))
            {
                results = results.Where(m => m.MultimediaCapacity == multimediaCapacity).ToList();
            }

            return results.OrderByDescending(o => o.id).Skip(pageIndex * pageSize).Take(pageSize).Select(s => new ProductForMobile
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
        }

        // PUT: api/Multimedia/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMultimediaModel(int id, MultimediaModel multimediaModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != multimediaModel.id)
            {
                return BadRequest();
            }

            db.Entry(multimediaModel).State = EntityState.Modified;

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