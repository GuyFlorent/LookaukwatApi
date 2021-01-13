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
    public class ModeController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Mode
        public IQueryable<ModeModel> GetProducts()
        {
            return db.Modes;
        }

        // GET: api/Mode/5
        [ResponseType(typeof(ModeViewModel))]
        public async Task<IHttpActionResult> GetModeModel(int id)
        {
            ModeModel modeModel = await db.Modes.FindAsync(id);
            if (modeModel == null)
            {
                return NotFound();
            }

            var ListeSimilar = db.Modes.Where(m => m.Category.CategoryName == modeModel.Category.CategoryName &&
            m.Town == modeModel.Town && m.SearchOrAskJob == modeModel.SearchOrAskJob &&
            m.RubriqueMode == modeModel.RubriqueMode && m.id != modeModel.id && m.RubriqueMode == modeModel.RubriqueMode).OrderBy(o => Guid.NewGuid()).
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


            ModeViewModel mode = new ModeViewModel
            {
                id = modeModel.id,
                Title = modeModel.Title,
                Description = modeModel.Description,
                DateAdd = modeModel.DateAdd,
                Images = modeModel.Images.Select(s=>s.ImageMobile).ToList(),
                UserName = modeModel.User.FirstName,
                UserEmail = modeModel.User.Email,
                UserPhone = modeModel.User.PhoneNumber,
                Town = modeModel.Town,
                Brand = modeModel.BrandMode,
                Type = modeModel.TypeMode,
                SearchOrAsk = modeModel.SearchOrAskJob,
                Color = modeModel.ColorMode,
                Price = modeModel.Price,
                Rubrique = modeModel.RubriqueMode,
                Size = modeModel.SizeMode,
                State = modeModel.StateMode,
                Street = modeModel.Street,
                Univers = modeModel.UniversMode,
                SimilarProduct = ListeSimilar
            };

            return Ok(mode);
        }

        // Result of Offer search Mode
        [Route("api/Mode/GetOfferModeSearchNumber")]
        public async Task<int> GetOfferModeSearchNumber(string categori, string town, string searchOrAskJob, int price, string rubriqueMode, string typeMode, string brandMode, string universMode, string sizeMode, string state, string colorMode)
        {

            var results = await db.Modes.
              Select(s => new SearchViewModel
              {
                  Category = s.Category.CategoryName,
                  Town = s.Town,
                  SearchOrAskJob = s.SearchOrAskJob,
                  Price = s.Price,
                  RubriqueMode = s.RubriqueMode,
                  TypeMode = s.TypeMode,
                  BrandMode = s.BrandMode,
                  UniversMode = s.UniversMode,
                  SizeMode = s.SizeMode,
                  State = s.StateMode,
                  ColorMode = s.ColorMode

              }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();

            if (price >= 0)
            {
                results = results.Where(m => m.Price <= price).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town))
            {
                results = results.Where(m => m.Town == town).ToList();
            }

            if (!string.IsNullOrWhiteSpace(rubriqueMode))
            {
                results = results.Where(m => m.RubriqueMode == rubriqueMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(typeMode))
            {
                results = results.Where(m => m.TypeMode == typeMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(brandMode))
            {
                results = results.Where(m => m.BrandMode == brandMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(universMode))
            {
                results = results.Where(m => m.UniversMode == universMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(sizeMode))
            {
                results = results.Where(m => m.SizeMode == sizeMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(state))
            {
                results = results.Where(m => m.State == state).ToList();
            }
            if (!string.IsNullOrWhiteSpace(colorMode))
            {
                results = results.Where(m => m.ColorMode == colorMode).ToList();
            }


            return results.Count;
        }

        // Result of Offer search Mode
        [Route("api/Mode/GetOfferModeSearch")]
        public async Task<List<ProductForMobile>> GetOfferModeSearch(string categori, string town, string searchOrAskJob, int price, string rubriqueMode, string typeMode, string brandMode, string universMode, string sizeMode, string state, string colorMode, int pageIndex, int pageSize)
        {

            var results = await db.Modes.
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
                  RubriqueMode = s.RubriqueMode,
                  TypeMode = s.TypeMode,
                  BrandMode = s.BrandMode,
                  UniversMode = s.UniversMode,
                  SizeMode = s.SizeMode,
                  State = s.StateMode,
                  ColorMode = s.ColorMode

              }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();

            if (price >= 0)
            {
                results = results.Where(m => m.Price <= price).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town))
            {
                results = results.Where(m => m.Town == town).ToList();
            }

            if (!string.IsNullOrWhiteSpace(rubriqueMode))
            {
                results = results.Where(m => m.RubriqueMode == rubriqueMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(typeMode))
            {
                results = results.Where(m => m.TypeMode == typeMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(brandMode))
            {
                results = results.Where(m => m.BrandMode == brandMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(universMode))
            {
                results = results.Where(m => m.UniversMode == universMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(sizeMode))
            {
                results = results.Where(m => m.SizeMode == sizeMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(state))
            {
                results = results.Where(m => m.State == state).ToList();
            }
            if (!string.IsNullOrWhiteSpace(colorMode))
            {
                results = results.Where(m => m.ColorMode == colorMode).ToList();
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

        // PUT: api/Mode/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutModeModel(int id, ModeModel modeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != modeModel.id)
            {
                return BadRequest();
            }

            db.Entry(modeModel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModeModelExists(id))
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

        // POST: api/Mode
        [ResponseType(typeof(ModeModel))]
        public async Task<IHttpActionResult> PostModeModel(ModeModel modeModel)
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
            modeModel.User = user;

            if (modeModel.Coordinate == null || (modeModel.Coordinate != null &&
                (modeModel.Coordinate.Lat == null || modeModel.Coordinate.Lon == null)))
            {
                modeModel.Coordinate = await CoordonateService.GetCoodinateAsync(modeModel.Town, modeModel.Street);
            }

            modeModel.Images = img;

            db.Modes.Add(modeModel);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = modeModel.id }, modeModel);
        }

        // DELETE: api/Mode/5
        [ResponseType(typeof(ModeModel))]
        public async Task<IHttpActionResult> DeleteModeModel(int id)
        {
            ModeModel modeModel = await db.Modes.FindAsync(id);
            if (modeModel == null)
            {
                return NotFound();
            }

            db.Products.Remove(modeModel);
            await db.SaveChangesAsync();

            return Ok(modeModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ModeModelExists(int id)
        {
            return db.Products.Count(e => e.id == id) > 0;
        }
    }
}