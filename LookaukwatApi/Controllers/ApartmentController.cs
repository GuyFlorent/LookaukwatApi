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
    public class ApartmentController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Apartment
        public IQueryable<ApartmentRentalModel> GetProducts()
        {
            return db.ApartmentRentals;
        }

        // GET: api/Apartment/5
        [ResponseType(typeof(ApartViewModel))]
        public async Task<IHttpActionResult> GetApartmentRentalModel(int id)
        {
            ApartmentRentalModel apartmentRentalModel = await db.ApartmentRentals.FindAsync(id);
            if (apartmentRentalModel == null)
            {
                return NotFound();
            }
            apartmentRentalModel.ViewNumber++;
            await db.SaveChangesAsync();

            var ListeSimilar = db.ApartmentRentals.Where(m =>
            m.Town == apartmentRentalModel.Town && m.SearchOrAskJob == apartmentRentalModel.SearchOrAskJob && m.id != apartmentRentalModel.id).OrderBy(o => Guid.NewGuid()).
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

            ApartViewModel apart = new ApartViewModel
            {
                id = apartmentRentalModel.id,
                Title = apartmentRentalModel.Title,
                Description = apartmentRentalModel.Description,
                DateAdd = apartmentRentalModel.DateAdd,
                Date = ConvertDate.Convert(apartmentRentalModel.DateAdd),
                Images = apartmentRentalModel.Images.Select(s => s.ImageMobile).ToList(),
                UserName = apartmentRentalModel.User.FirstName,
                UserEmail = apartmentRentalModel.User.Email,
                UserPhone = apartmentRentalModel.User.PhoneNumber,
                Town = apartmentRentalModel.Town,
                FurnitureOrNot = apartmentRentalModel.FurnitureOrNot,
                Type = apartmentRentalModel.Type,
                SearchOrAsk = apartmentRentalModel.SearchOrAskJob,
                ApartSurface = apartmentRentalModel.ApartSurface,
                Price = apartmentRentalModel.Price,
                RoomNumber = apartmentRentalModel.RoomNumber,
                Street = apartmentRentalModel.Street,
                SimilarProduct = Liste,
                ViewNumber = apartmentRentalModel.ViewNumber,
                Lat = apartmentRentalModel.Coordinate.Lat,
                Lon = apartmentRentalModel.Coordinate.Lon,
            };

            return Ok(apart);
        }

        [HttpGet]
        [Route("api/Apartment/GetApartCritere")]
        [ResponseType(typeof(ApartViewModel))]
        public async Task<IHttpActionResult> GetApartCritere(int id)
        {
            ApartmentRentalModel apartmentRentalModel = await db.ApartmentRentals.FindAsync(id);
            if (apartmentRentalModel == null)
            {
                return NotFound();
            }

            ApartViewModel apart = new ApartViewModel
            {
                id = apartmentRentalModel.id,
                Type = apartmentRentalModel.Type,
                RoomNumber = apartmentRentalModel.RoomNumber,
                ApartSurface = apartmentRentalModel.ApartSurface,
                SearchOrAsk = apartmentRentalModel.SearchOrAskJob,
                FurnitureOrNot = apartmentRentalModel.FurnitureOrNot,
                Price = apartmentRentalModel.Price

            };
            return Ok(apart);
        }


        // Result of Offer search Apartment
        [Route("api/Apartment/GetOfferAppartSearchNumber")]
        public async Task<int> GetOfferAppartSearchNumber(string categori, string town, string searchOrAskJob,int price, int roomNumberAppart, string furnitureOrNotAppart, string typeAppart, int apartSurfaceAppart)
        {

            var results = await db.ApartmentRentals.
              Select(s => new SearchViewModel
              {
                  Category = s.Category.CategoryName,
                  Town = s.Town,
                  SearchOrAskJob = s.SearchOrAskJob,
                  Price = s.Price,
                  RoomNumberAppart = s.RoomNumber,
                  FurnitureOrNotAppart = s.FurnitureOrNot,
                  TypeAppart = s.Type,
                  ApartSurfaceAppart = s.ApartSurface

              }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();

            if (!string.IsNullOrWhiteSpace(typeAppart) && typeAppart != "Tout")
            {
                results = results.Where(m => m.TypeAppart != null && m.TypeAppart == typeAppart).ToList();
            }

            if (price >= 0 && price < 1000000)
            {
                results = results.Where(m => m.Price <= price).ToList();
            }

            if (!string.IsNullOrWhiteSpace(furnitureOrNotAppart) && typeAppart !="Terrain à vendre" && furnitureOrNotAppart != "Tout")
            {
                results = results.Where(m => m.FurnitureOrNotAppart != null && m.FurnitureOrNotAppart == furnitureOrNotAppart).ToList();

                if (roomNumberAppart >= 0)
                {
                    results = results.Where(m => m.RoomNumberAppart <= roomNumberAppart).ToList();
                }
            }

           
            if (apartSurfaceAppart >= 0 && apartSurfaceAppart < 2000)
            {
                results = results.Where(m => m.ApartSurfaceAppart <= apartSurfaceAppart).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town) && town != "Toutes")
            {
                results = results.Where(m => m.Town == town).ToList();
            }

            
            return results.Count;
        }

        // Result of Offer search Apartment
        [Route("api/Apartment/GetOfferAppartSearch")]
        public async Task<List<ProductForMobile>> GetOfferAppartSearch(string categori, string town, string searchOrAskJob, int price, int roomNumberAppart, string furnitureOrNotAppart, string typeAppart, int apartSurfaceAppart, int pageIndex, int pageSize, string sortBy)
        {
            List<SearchViewModel> results = new List<SearchViewModel>();

            switch (sortBy)
            {
                case "MostRecent":

                    results = await db.ApartmentRentals.OrderByDescending(o => o.id).
            Select(s => new SearchViewModel
            {
                id = s.id,
                Title = s.Title,
                Images = s.Images,
                DateAdd = s.DateAdd,
                Category = s.Category.CategoryName,
                Town = s.Town,
                SearchOrAskJob = s.SearchOrAskJob,
                Price = s.Price,
                RoomNumberAppart = s.RoomNumber,
                FurnitureOrNotAppart = s.FurnitureOrNot,
                TypeAppart = s.Type,
                ApartSurfaceAppart = s.ApartSurface,
                ViewNumber = s.ViewNumber

            }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                case "MostOld":

                    results = await db.ApartmentRentals.OrderBy(o => o.id).
            Select(s => new SearchViewModel
            {
                id = s.id,
                Title = s.Title,
                Images = s.Images,
                DateAdd = s.DateAdd,
                Category = s.Category.CategoryName,
                Town = s.Town,
                SearchOrAskJob = s.SearchOrAskJob,
                Price = s.Price,
                RoomNumberAppart = s.RoomNumber,
                FurnitureOrNotAppart = s.FurnitureOrNot,
                TypeAppart = s.Type,
                ApartSurfaceAppart = s.ApartSurface,
                ViewNumber = s.ViewNumber

            }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                case "LowerPrice":

                    results = await db.ApartmentRentals.OrderBy(o => o.Price).
            Select(s => new SearchViewModel
            {
                id = s.id,
                Title = s.Title,
                Images = s.Images,
                DateAdd = s.DateAdd,
                Category = s.Category.CategoryName,
                Town = s.Town,
                SearchOrAskJob = s.SearchOrAskJob,
                Price = s.Price,
                RoomNumberAppart = s.RoomNumber,
                FurnitureOrNotAppart = s.FurnitureOrNot,
                TypeAppart = s.Type,
                ApartSurfaceAppart = s.ApartSurface,
                ViewNumber = s.ViewNumber

            }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                case "HeigherPrice":

                    results = await db.ApartmentRentals.OrderByDescending(o => o.Price).
            Select(s => new SearchViewModel
            {
                id = s.id,
                Title = s.Title,
                Images = s.Images,
                DateAdd = s.DateAdd,
                Category = s.Category.CategoryName,
                Town = s.Town,
                SearchOrAskJob = s.SearchOrAskJob,
                Price = s.Price,
                RoomNumberAppart = s.RoomNumber,
                FurnitureOrNotAppart = s.FurnitureOrNot,
                TypeAppart = s.Type,
                ApartSurfaceAppart = s.ApartSurface,
                ViewNumber = s.ViewNumber

            }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                default:

                    results = await db.ApartmentRentals.OrderByDescending(o =>o.id).
             Select(s => new SearchViewModel
             {
                 id = s.id,
                 Title = s.Title,
                 Images = s.Images,
                 DateAdd = s.DateAdd,
                 Category = s.Category.CategoryName,
                 Town = s.Town,
                 SearchOrAskJob = s.SearchOrAskJob,
                 Price = s.Price,
                 RoomNumberAppart = s.RoomNumber,
                 FurnitureOrNotAppart = s.FurnitureOrNot,
                 TypeAppart = s.Type,
                 ApartSurfaceAppart = s.ApartSurface,
                 ViewNumber = s.ViewNumber

             }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
            }

            

            if (!string.IsNullOrWhiteSpace(typeAppart) && typeAppart != "Tout")
            {
                results = results.Where(m => m.TypeAppart != null && m.TypeAppart == typeAppart).ToList();
            }

            if (price >= 0)
            {
                results = results.Where(m => m.Price <= price).ToList();
            }

            if (!string.IsNullOrWhiteSpace(furnitureOrNotAppart) && typeAppart != "Terrain à vendre" && furnitureOrNotAppart != "Tout")
            {
                results = results.Where(m => m.FurnitureOrNotAppart != null && m.FurnitureOrNotAppart == furnitureOrNotAppart).ToList();

                if (roomNumberAppart >= 0)
                {
                    results = results.Where(m => m.RoomNumberAppart <= roomNumberAppart).ToList();
                }
            }


            if (apartSurfaceAppart >= 0)
            {
                results = results.Where(m => m.ApartSurfaceAppart <= apartSurfaceAppart).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town) && town != "Toutes")
            {
                results = results.Where(m => m.Town == town).ToList();
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
                ViewNumber = s.ViewNumber
            }).ToList();

            return List;
        }


        // PUT: api/Apartment/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutApartmentRentalModel(int id, ApartCritereViewModel apartmentRentalModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != apartmentRentalModel.id)
            {
                return BadRequest();
            }

            var apart = await db.ApartmentRentals.FirstOrDefaultAsync(m => m.id == id);


            apart.SearchOrAskJob = apartmentRentalModel.SearchOrAsk;
            apart.Price = apartmentRentalModel.Price;
            apart.Type = apartmentRentalModel.Type;
            apart.RoomNumber = apartmentRentalModel.RoomNumber;
            apart.FurnitureOrNot = apartmentRentalModel.FurnitureOrNot;
            apart.ApartSurface = apartmentRentalModel.ApartSurface;
            

            db.Entry(apart).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApartmentRentalModelExists(id))
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

        // POST: api/Apartment
        [ResponseType(typeof(ApartmentRentalModel))]
        public async Task<IHttpActionResult> PostApartmentRentalModel(ApartmentRentalModel apartmentRentalModel)
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
            apartmentRentalModel.User = user;
            apartmentRentalModel.DateAdd = DateTime.UtcNow;
            if (apartmentRentalModel.Coordinate == null || (apartmentRentalModel.Coordinate != null &&
                (apartmentRentalModel.Coordinate.Lat == null || apartmentRentalModel.Coordinate.Lon == null)))
            {
                apartmentRentalModel.Coordinate = await CoordonateService.GetCoodinateAsync(apartmentRentalModel.Town, apartmentRentalModel.Street);

            }



            apartmentRentalModel.Images = img;

            db.Products.Add(apartmentRentalModel);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = apartmentRentalModel.id }, apartmentRentalModel);
        }

        // DELETE: api/Apartment/5
        [ResponseType(typeof(ApartmentRentalModel))]
        public async Task<IHttpActionResult> DeleteApartmentRentalModel(int id)
        {
            ApartmentRentalModel apartmentRentalModel = await db.ApartmentRentals.FindAsync(id);
            if (apartmentRentalModel == null)
            {
                return NotFound();
            }

            db.Products.Remove(apartmentRentalModel);
            await db.SaveChangesAsync();

            return Ok(apartmentRentalModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ApartmentRentalModelExists(int id)
        {
            return db.Products.Count(e => e.id == id) > 0;
        }
    }
}