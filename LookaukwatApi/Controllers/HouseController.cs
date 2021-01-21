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
    public class HouseController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/House
        public IQueryable<HouseModel> GetProducts()
        {
            return db.Houses;
        }

        // GET: api/House/5
        [ResponseType(typeof(HouseViewModel))]
        public async Task<IHttpActionResult> GetHouseModel(int id)
        {
            HouseModel houseModel = await db.Houses.FindAsync(id);
            if (houseModel == null)
            {
                return NotFound();
            }
            houseModel.ViewNumber++;
            await db.SaveChangesAsync();

            var ListeSimilar = db.Houses.Where(m => m.Category.CategoryName == houseModel.Category.CategoryName &&
           m.Town == houseModel.Town && m.SearchOrAskJob == houseModel.SearchOrAskJob &&
           m.SearchOrAskJob == houseModel.SearchOrAskJob && m.id != houseModel.id && m.RubriqueHouse == houseModel.RubriqueHouse).OrderBy(o => Guid.NewGuid()).
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


            HouseViewModel house = new HouseViewModel
            {
                id = houseModel.id,
                Title = houseModel.Title,
                Description = houseModel.Description,
                DateAdd = houseModel.DateAdd,
                Date = ConvertDate.Convert(houseModel.DateAdd),
                Images = houseModel.Images.Select(s => s.ImageMobile).ToList(),
                UserName = houseModel.User.FirstName,
                UserEmail = houseModel.User.Email,
                UserPhone = houseModel.User.PhoneNumber,
                Town = houseModel.Town,
                ColorHouse = houseModel.ColorHouse,
                FabricMaterialeHouse = houseModel.FabricMaterialeHouse,
                SearchOrAsk = houseModel.SearchOrAskJob,
                RubriqueHouse = houseModel.RubriqueHouse,
                Price = houseModel.Price,
                StateHouse = houseModel.StateHouse,
                TypeHouse = houseModel.TypeHouse,
                Street = houseModel.Street,
                SimilarProduct = Liste,
                ViewNumber = houseModel.ViewNumber,
                Lat = houseModel.Coordinate.Lat,
                Lon = houseModel.Coordinate.Lon,
            };


            return Ok(house);
        }

        // Result of Offer search House
        [Route("api/House/GetOfferHouseSearchNumber")]
        public async Task<int> GetOfferHouseSearchNumber(string categori, string town, string searchOrAskJob, int price, string rubriqueHouse, string typeHouse, string fabricMaterialHouse, string stateHouse, string colorHouse)
        {

            var results = await db.Houses.
              Select(s => new SearchViewModel
              {
                  Category = s.Category.CategoryName,
                  Town = s.Town,
                  SearchOrAskJob = s.SearchOrAskJob,
                  Price = s.Price,
                  TypeHouse = s.TypeHouse,
                  RubriqueHouse = s.RubriqueHouse,
                  ColorHouse = s.ColorHouse,
                  FabricMaterialHouse =s.FabricMaterialeHouse,
                  StateHouse = s.StateHouse

              }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();

            if (price >= 0 && price < 100000)
            {
                results = results.Where(m => m.Price <= price).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town) && town != "Toutes")
            {
                results = results.Where(m => m.Town == town).ToList();
            }

            if (!string.IsNullOrWhiteSpace(typeHouse) && typeHouse != "Tout")
            {
                results = results.Where(m => m.TypeHouse == typeHouse).ToList();
            }

            if (!string.IsNullOrWhiteSpace(rubriqueHouse) && rubriqueHouse != "Toutes")
            {
                results = results.Where(m => m.RubriqueHouse == rubriqueHouse).ToList();
            }

            if (!string.IsNullOrWhiteSpace(colorHouse) && colorHouse != "Toutes")
            {
                results = results.Where(m => m.ColorHouse == colorHouse).ToList();
            }

            if (!string.IsNullOrWhiteSpace(fabricMaterialHouse) && fabricMaterialHouse != "Tout")
            {
                results = results.Where(m => m.FabricMaterialHouse == fabricMaterialHouse).ToList();
            }

            if (!string.IsNullOrWhiteSpace(stateHouse) && stateHouse != "Tout")
            {
                results = results.Where(m => m.StateHouse == stateHouse).ToList();
            }


            return results.Count;
        }


        // Result list House of Offer search House
        [Route("api/House/GetOfferHouseSearch")]
        public async Task<List<ProductForMobile>> GetOfferHouseSearch(string categori, string town, string searchOrAskJob, int price, string rubriqueHouse, string typeHouse, string fabricMaterialHouse, string stateHouse, string colorHouse, int pageIndex, int pageSize, string sortBy)
        {
            List<SearchViewModel> results = new List<SearchViewModel>();

            switch (sortBy)
            {
                case "MostRecent":

                    results = await db.Houses.OrderByDescending(o => o.id).
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
                TypeHouse = s.TypeHouse,
                RubriqueHouse = s.RubriqueHouse,
                ColorHouse = s.ColorHouse,
                FabricMaterialHouse = s.FabricMaterialeHouse,
                StateHouse = s.StateHouse

            }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                case "MostOld":

                    results = await db.Houses.OrderBy(o => o.id).
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
                TypeHouse = s.TypeHouse,
                RubriqueHouse = s.RubriqueHouse,
                ColorHouse = s.ColorHouse,
                FabricMaterialHouse = s.FabricMaterialeHouse,
                StateHouse = s.StateHouse

            }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                case "LowerPrice":

                    results = await db.Houses.OrderBy(o => o.Price).
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
                TypeHouse = s.TypeHouse,
                RubriqueHouse = s.RubriqueHouse,
                ColorHouse = s.ColorHouse,
                FabricMaterialHouse = s.FabricMaterialeHouse,
                StateHouse = s.StateHouse

            }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                case "HeigherPrice":
                    results = await db.Houses.OrderByDescending(o => o.Price).
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
                TypeHouse = s.TypeHouse,
                RubriqueHouse = s.RubriqueHouse,
                ColorHouse = s.ColorHouse,
                FabricMaterialHouse = s.FabricMaterialeHouse,
                StateHouse = s.StateHouse

            }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                default:

                    results = await db.Houses.OrderByDescending(o =>o.id).
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
                 TypeHouse = s.TypeHouse,
                 RubriqueHouse = s.RubriqueHouse,
                 ColorHouse = s.ColorHouse,
                 FabricMaterialHouse = s.FabricMaterialeHouse,
                 StateHouse = s.StateHouse

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

            if (!string.IsNullOrWhiteSpace(typeHouse) && typeHouse != "Tout")
            {
                results = results.Where(m => m.TypeHouse == typeHouse).ToList();
            }

            if (!string.IsNullOrWhiteSpace(rubriqueHouse) && rubriqueHouse != "Toutes")
            {
                results = results.Where(m => m.RubriqueHouse == rubriqueHouse).ToList();
            }

            if (!string.IsNullOrWhiteSpace(colorHouse) && colorHouse != "Toutes")
            {
                results = results.Where(m => m.ColorHouse == colorHouse).ToList();
            }

            if (!string.IsNullOrWhiteSpace(fabricMaterialHouse) && fabricMaterialHouse != "Tout")
            {
                results = results.Where(m => m.FabricMaterialHouse == fabricMaterialHouse).ToList();
            }

            if (!string.IsNullOrWhiteSpace(stateHouse) && stateHouse != "Tout")
            {
                results = results.Where(m => m.StateHouse == stateHouse).ToList();
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

        // PUT: api/House/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHouseModel(int id, HouseModel houseModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != houseModel.id)
            {
                return BadRequest();
            }

            db.Entry(houseModel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HouseModelExists(id))
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

        // POST: api/House
        [ResponseType(typeof(HouseModel))]
        public async Task<IHttpActionResult> PostHouseModel(HouseModel houseModel)
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
            houseModel.User = user;
            houseModel.DateAdd = DateTime.UtcNow;
            if (houseModel.Coordinate == null || (houseModel.Coordinate != null &&
                (houseModel.Coordinate.Lat == null || houseModel.Coordinate.Lon == null)))
            {
                houseModel.Coordinate = await CoordonateService.GetCoodinateAsync(houseModel.Town, houseModel.Street);
            }

            houseModel.Images = img;

            db.Houses.Add(houseModel);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = houseModel.id }, houseModel);
        }

        // DELETE: api/House/5
        [ResponseType(typeof(HouseModel))]
        public async Task<IHttpActionResult> DeleteHouseModel(int id)
        {
            HouseModel houseModel = await db.Houses.FindAsync(id);
            if (houseModel == null)
            {
                return NotFound();
            }

            db.Products.Remove(houseModel);
            await db.SaveChangesAsync();

            return Ok(houseModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HouseModelExists(int id)
        {
            return db.Products.Count(e => e.id == id) > 0;
        }
    }
}