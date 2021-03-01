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

            modeModel.ViewNumber++;
            await db.SaveChangesAsync();

            var ListeSimilar = db.Modes.Where(m => m.Town == modeModel.Town && m.SearchOrAskJob == modeModel.SearchOrAskJob && m.RubriqueMode == modeModel.RubriqueMode
            && m.id != modeModel.id && m.TypeMode == modeModel.TypeMode).OrderBy(o => Guid.NewGuid()).
            Take(6).Select(s => new SimilarProductViewModel
            {
                id = s.id,
                Category = s.Category.CategoryName,
                Title = s.Title,
                Price = s.Price,
                Town = s.Town,
                DateAdd = s.DateAdd,
                Image = s.Images.Select(m => m.ImageMobile).FirstOrDefault(),
                NumberImages = s.Images.Count,
                IsLookaukwat = s.IsLookaukwat,
               
            }).ToList();

            List<SimilarProductViewModel> Liste = new List<SimilarProductViewModel>();
            Liste = ListeSimilar;
            //foreach (var item in ListeSimilar)
            //{
            //    item.Date = ConvertDate.Convert(item.DateAdd);
            //    Liste.Add(item);
            //}


            ModeViewModel mode = new ModeViewModel
            {
                id = modeModel.id,
                Title = modeModel.Title,
                Description = modeModel.Description,
                DateAdd = modeModel.DateAdd,
                Date = ConvertDate.Convert(modeModel.DateAdd),
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
                SimilarProduct = Liste,
                Lat = modeModel.Coordinate.Lat,
                Lon = modeModel.Coordinate.Lon,
                ViewNumber = modeModel.ViewNumber,
                IsLookaukwat = modeModel.IsLookaukwat,
                Stock = modeModel.Stock
            };

            return Ok(mode);
        }


        [HttpGet]
        [Route("api/Mode/GetModeCritere")]
        [ResponseType(typeof(ModeViewModel))]
        public async Task<IHttpActionResult> GetModeCritere(int id)
        {
            ModeModel Mode = await db.Modes.FindAsync(id);
            if (Mode == null)
            {
                return NotFound();
            }

            ModeViewModel mode = new ModeViewModel
            {
                id = Mode.id,
                Price = Mode.Price,
                SearchOrAsk = Mode.SearchOrAskJob,
                Rubrique = Mode.RubriqueMode,
                Brand = Mode.BrandMode,
                Type = Mode.TypeMode,
                Univers = Mode.UniversMode,
                Size = Mode.SizeMode,
                Color = Mode.ColorMode,
                State = Mode.StateMode
               
            };
            return Ok(mode);
        }

        // Result of Offer search Mode
        [Route("api/Mode/GetOfferModeSearchNumber")]
        public async Task<int> GetOfferModeSearchNumber(string categori, string town, string searchOrAskJob, int price, string rubriqueMode, string typeMode, string brandMode, string universMode, string sizeMode, string state, string colorMode, bool isParticulier, bool isLookaukwat)
        {

            var results = await db.Modes.Where(m => m.IsActive && m.Category.CategoryName == categori && m.SearchOrAskJob == searchOrAskJob).
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
                  ColorMode = s.ColorMode,
                  IsLookaukwat = s.IsLookaukwat,
                  IsParticulier = s.IsParticulier

              }).ToListAsync();

            if (isLookaukwat && isParticulier)
            {
                results = results.Where(m => m.IsLookaukwat || m.IsParticulier).ToList();
            }
            else if (isLookaukwat)
            {
                results = results.Where(m => m.IsLookaukwat).ToList();
            }
            else if (isParticulier)
            {
                results = results.Where(m => m.IsParticulier).ToList();
            }

            if (price >= 0 && price < 100000)
            {
                results = results.Where(m => m.Price <= price).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town) && town != "Toutes")
            {
                results = results.Where(m => m.Town == town).ToList();
            }

            if (!string.IsNullOrWhiteSpace(rubriqueMode) && rubriqueMode != "Toutes")
            {
                results = results.Where(m => m.RubriqueMode == rubriqueMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(typeMode) && typeMode != "Tout")
            {
                results = results.Where(m => m.TypeMode!=null && m.TypeMode == typeMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(brandMode) && brandMode != "Toutes")
            {
                results = results.Where(m => m.BrandMode != null && m.BrandMode == brandMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(universMode) && universMode != "Tout")
            {
                results = results.Where(m =>  m.UniversMode != null && m.UniversMode == universMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(sizeMode) && sizeMode != "Toutes")
            {
                results = results.Where(m => m.SizeMode != null && m.SizeMode == sizeMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(state) && state != "Tout")
            {
                results = results.Where(m => m.State != null && m.State == state).ToList();
            }
            if (!string.IsNullOrWhiteSpace(colorMode) && colorMode != "Toutes")
            {
                results = results.Where(m => m.ColorMode != null && m.ColorMode == colorMode).ToList();
            }


            return results.Count;
        }

        // Result of Offer search Mode
        [Route("api/Mode/GetOfferModeSearch")]
        public async Task<List<ProductForMobile>> GetOfferModeSearch(string categori, string town, string searchOrAskJob, int price, string rubriqueMode, string typeMode, string brandMode, string universMode, string sizeMode, string state, string colorMode, int pageIndex, int pageSize, string sortBy, bool isParticulier, bool isLookaukwat)
        {
            List<SearchViewModel> results = new List<SearchViewModel>();

            switch (sortBy)
            {
                case "MostRecent":

                    results = await db.Modes.OrderByDescending(o => o.id).
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
                ColorMode = s.ColorMode,
                IsLookaukwat = s.IsLookaukwat,
                IsParticulier = s.IsParticulier,
                IsActive = s.IsActive

            }).Where(m => m.IsActive && m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();

                    break;
                case "MostOld":

                    results = await db.Modes.OrderBy(o => o.id).
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
                ColorMode = s.ColorMode,
                IsLookaukwat = s.IsLookaukwat,
                IsParticulier = s.IsParticulier,
                IsActive = s.IsActive

            }).Where(m => m.IsActive && m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();

                    break;
                case "LowerPrice":

                    results = await db.Modes.OrderBy(o => o.Price).
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
                ColorMode = s.ColorMode,
                IsLookaukwat = s.IsLookaukwat,
                IsParticulier = s.IsParticulier,
                IsActive = s.IsActive

            }).Where(m => m.IsActive && m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();

                    break;
                case "HeigherPrice":

                    results = await db.Modes.OrderByDescending(o => o.Price).
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
                ColorMode = s.ColorMode,
                IsLookaukwat = s.IsLookaukwat,
                IsParticulier = s.IsParticulier,
                IsActive = s.IsActive

            }).Where(m => m.IsActive && m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                default:

                    results = await db.Modes.OrderByDescending(o=>o.id).
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
                 ColorMode = s.ColorMode,
                 IsLookaukwat = s.IsLookaukwat,
                 IsParticulier = s.IsParticulier,
                 IsActive = s.IsActive

             }).Where(m => m.IsActive && m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
            }

            if (isLookaukwat && isParticulier)
            {
                results = results.Where(m => m.IsLookaukwat || m.IsParticulier).ToList();
            }
            else if (isLookaukwat)
            {
                results = results.Where(m => m.IsLookaukwat).ToList();
            }
            else if (isParticulier)
            {
                results = results.Where(m => m.IsParticulier).ToList();
            }

            if (price >= 0 && price < 100000)
            {
                results = results.Where(m => m.Price <= price).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town) && town != "Toutes")
            {
                results = results.Where(m => m.Town == town).ToList();
            }

            if (!string.IsNullOrWhiteSpace(rubriqueMode) && rubriqueMode != "Toutes")
            {
                results = results.Where(m => m.RubriqueMode == rubriqueMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(typeMode) && typeMode != "Tout")
            {
                results = results.Where(m => m.TypeMode != null && m.TypeMode == typeMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(brandMode) && brandMode != "Toutes")
            {
                results = results.Where(m => m.BrandMode != null && m.BrandMode == brandMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(universMode) && universMode != "Tout")
            {
                results = results.Where(m => m.UniversMode != null && m.UniversMode == universMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(sizeMode) && sizeMode != "Toutes")
            {
                results = results.Where(m => m.SizeMode != null && m.SizeMode == sizeMode).ToList();
            }

            if (!string.IsNullOrWhiteSpace(state) && state != "Tout")
            {
                results = results.Where(m => m.State != null && m.State == state).ToList();
            }
            if (!string.IsNullOrWhiteSpace(colorMode) && colorMode != "Toutes")
            {
                results = results.Where(m => m.Color != null && m.ColorMode == colorMode).ToList();
            }


            List<SearchViewModel> Liste = new List<SearchViewModel>();

            Liste = results.ToList();
            var List = Liste.Skip(pageIndex * pageSize).Take(pageSize).Select(s => new ProductForMobile
            {
                Title = s.Title,
                Town = s.Town,
                DateAdd = s.DateAdd,
                Category = s.Category,
                Image = s.Images.Select(im => im.ImageMobile).FirstOrDefault(),
                id = s.id,
                Price = s.Price,
                Date = s.Date,
                ViewNumber = s.ViewNumber,
                NumberImages = s.Images.Count
            }).ToList();

            return List;
        }

        // PUT: api/Mode/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutModeModel(int id, ModeCritereViewModel modeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != modeModel.id)
            {
                return BadRequest();
            }

            var mode = await db.Modes.FirstOrDefaultAsync(m => m.id == id);


            mode.SearchOrAskJob = modeModel.SearchOrAsk;
            mode.RubriqueMode = modeModel.Rubrique;
            mode.BrandMode = modeModel.Brand;
            mode.TypeMode = modeModel.Type;
            mode.UniversMode = modeModel.Univers;
            mode.SizeMode = modeModel.Size;
            mode.ColorMode = modeModel.Color;
            mode.StateMode = modeModel.State;
            mode.Price = modeModel.Price;

            db.Entry(mode).State = EntityState.Modified;

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

            //Know if its lookaukwat or not
            if (User.IsInRole(MyRoleConstant.RoleAdmin))
            {
                modeModel.IsLookaukwat = true;
            }
            else
            {
                modeModel.IsParticulier = true;
            }

            string UserId = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(m => m.Id == UserId);
            user.Date_First_Publish = DateTime.UtcNow;
            List<ImageProcductModel> img = new List<ImageProcductModel>();
            var im = new ImageProcductModel() { id = Guid.NewGuid(), Image = "https://particulier-employeur.fr/wp-content/themes/fepem/img/general/avatar.png", ImageMobile = "https://particulier-employeur.fr/wp-content/themes/fepem/img/general/avatar.png" };
            img.Add(im);
            modeModel.User = user;
            modeModel.DateAdd = DateTime.UtcNow;
            modeModel.IsActive = true;
            modeModel.Stock = 1;
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