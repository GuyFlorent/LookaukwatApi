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
    public class VehiculeController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: api/Vehicule
        public IQueryable<VehiculeModel> GetProducts()
        {
            return db.Vehicules;
        }

        // GET: api/Vehicule/5
        [ResponseType(typeof(VehiculeViewModel))]
        public async Task<IHttpActionResult> GetVehiculeModel(int id)
        {
            VehiculeModel vehiculeModel = await db.Vehicules.FindAsync(id);
            if (vehiculeModel == null)
            {
                return NotFound();
            }
            vehiculeModel.ViewNumber++;
            await db.SaveChangesAsync();

            var ListeSimilar = db.Vehicules.Where(m => m.Category.CategoryName == vehiculeModel.Category.CategoryName &&
          m.Town == vehiculeModel.Town && m.SearchOrAskJob == vehiculeModel.SearchOrAskJob &&
          m.SearchOrAskJob == vehiculeModel.SearchOrAskJob && m.id != vehiculeModel.id && m.RubriqueVehicule == vehiculeModel.RubriqueVehicule).OrderBy(o => Guid.NewGuid()).
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

            VehiculeViewModel vehicule = new VehiculeViewModel
            {
                id = vehiculeModel.id,
                Title = vehiculeModel.Title,
                Description = vehiculeModel.Description,
                DateAdd = vehiculeModel.DateAdd,
                Date = ConvertDate.Convert(vehiculeModel.DateAdd),
                Images = vehiculeModel.Images.Select(s => s.ImageMobile).ToList(),
                UserName = vehiculeModel.User.FirstName,
                UserEmail = vehiculeModel.User.Email,
                UserPhone = vehiculeModel.User.PhoneNumber,
                Town = vehiculeModel.Town,
                FirstYearVehicule = vehiculeModel.FirstYearVehicule,
                NumberOfDoorVehicule = vehiculeModel.NumberOfDoorVehicule,
                SearchOrAsk = vehiculeModel.SearchOrAskJob,
                BrandVehicule = vehiculeModel.BrandVehicule,
                Price = vehiculeModel.Price,
                ColorVehicule = vehiculeModel.ColorVehicule,
                GearBoxVehicule = vehiculeModel.GearBoxVehicule,
                ModelVehicule = vehiculeModel.ModelVehicule,
                MileageVehicule = vehiculeModel.MileageVehicule,
                PetrolVehicule = vehiculeModel.PetrolVehicule,
                RubriqueVehicule = vehiculeModel.RubriqueVehicule,
                StateVehicule = vehiculeModel.StateVehicule,
                TypeVehicule = vehiculeModel.TypeVehicule,
                YearVehicule = vehiculeModel.YearVehicule,
                Street = vehiculeModel.Street,
                SimilarProduct = Liste,
                ViewNumber = vehiculeModel.ViewNumber,
                Lat = vehiculeModel.Coordinate.Lat,
                Lon = vehiculeModel.Coordinate.Lon,
            };

            return Ok(vehicule);
        }

        [HttpGet]
        [Route("api/Vehicule/GetVehiculediaCritere")]
        [ResponseType(typeof(VehiculeViewModel))]
        public async Task<IHttpActionResult> GetVehiculediaCritere(int id)
        {
            VehiculeModel Vehi = await db.Vehicules.FindAsync(id);
            if (Vehi == null)
            {
                return NotFound();
            }

            VehiculeViewModel vehicule = new VehiculeViewModel
            {
                id = Vehi.id,
                Price = Vehi.Price,
                SearchOrAsk = Vehi.SearchOrAskJob,
                RubriqueVehicule = Vehi.RubriqueVehicule,
                BrandVehicule = Vehi.BrandVehicule,
                TypeVehicule = Vehi.TypeVehicule,
                ColorVehicule = Vehi.ColorVehicule,
                YearVehicule = Vehi.YearVehicule,
                FirstYearVehicule = Vehi.FirstYearVehicule,
                MileageVehicule = Vehi.MileageVehicule,
                NumberOfDoorVehicule = Vehi.NumberOfDoorVehicule,
                GearBoxVehicule = Vehi.GearBoxVehicule,
                ModelVehicule = Vehi.ModelVehicule,
                StateVehicule = Vehi.StateVehicule,
                PetrolVehicule = Vehi.PetrolVehicule

            };
            return Ok(vehicule);
        }

        // Result of Offer search Vehicule
        [Route("api/Vehicule/GetOfferVehiculeSearchNumber")]
        public async Task<int> GetOfferVehiculeSearchNumber(string categori, string town, string searchOrAskJob, int price, string vehiculeRubrique, string vehiculeBrand, string vehiculeModel, string vehiculeType, string petrol, string year, string mileage, string numberOfDoor, string gearBox, string vehiculestate, string color)
        {

            var results = await db.Vehicules.
              Select(s => new SearchViewModel
              {
                  Category = s.Category.CategoryName,
                  Town = s.Town,
                  SearchOrAskJob = s.SearchOrAskJob,
                  Price = s.Price,
                  VehiculeBrand = s.BrandVehicule,
                  VehiculeModel = s.ModelVehicule,
                  VehiculeType = s.TypeVehicule,
                  Petrol = s.PetrolVehicule,
                  Year = s.YearVehicule,
                  Mileage = s.MileageVehicule,
                  NumberOfDoor = s.NumberOfDoorVehicule,
                  GearBox = s.GearBoxVehicule,
                  Color = s.ColorVehicule,
                  Vehiculestate = s.StateVehicule,
                  VehiculeRubrique = s.RubriqueVehicule

              }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();

            if (price >= 0 && price < 2000000)
            {
                results = results.Where(m => m.Price <= price).ToList();
            }


            if (!string.IsNullOrWhiteSpace(vehiculeRubrique) && vehiculeRubrique != "Toutes")
            {
                results = results.Where(m => m.VehiculeRubrique == vehiculeRubrique).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town) && town != "Toutes")
            {
                results = results.Where(m => m.Town == town).ToList();
            }

            if (!string.IsNullOrWhiteSpace(year))
            {
                results = results.Where(m => Convert.ToInt32( m.Year) <= Convert.ToInt32(year)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(mileage) && Convert.ToInt32(mileage) <300000)
            {
                results = results.Where(m => Convert.ToInt32(m.Mileage) <= Convert.ToInt32(mileage)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(vehiculeBrand) && vehiculeBrand != "Toutes")
            {
                results = results.Where(m => m.VehiculeBrand == vehiculeBrand).ToList();
            }

            if (!string.IsNullOrWhiteSpace(vehiculeModel) && vehiculeModel != "Tout")
            {
                results = results.Where(m => m.VehiculeModel == vehiculeModel).ToList();
            }

            if (!string.IsNullOrWhiteSpace(vehiculeType) && vehiculeType != "Tout")
            {
                results = results.Where(m => m.VehiculeType == vehiculeType).ToList();
            }

            if (!string.IsNullOrWhiteSpace(petrol) && petrol != "Tout")
            {
                results = results.Where(m => m.Petrol == petrol).ToList();
            }

            if (!string.IsNullOrWhiteSpace(numberOfDoor) && numberOfDoor != "Toutes")
            {
                results = results.Where(m => m.NumberOfDoor == numberOfDoor).ToList();
            }

            if (!string.IsNullOrWhiteSpace(gearBox) && gearBox != "Toutes")
            {
                results = results.Where(m => m.GearBox == gearBox).ToList();
            }
            if (!string.IsNullOrWhiteSpace(color) && color != "Toutes")
            {
                results = results.Where(m => m.Color == color).ToList();
            }

            if (!string.IsNullOrWhiteSpace(vehiculestate) && vehiculestate != "Tout")
            {
                results = results.Where(m => m.Vehiculestate == vehiculestate).ToList();
            }


            return results.Count;
        }


        // Result of Offer search Vehicule
        [Route("api/Vehicule/GetOfferVehiculeSearch")]
        public async Task<List<ProductForMobile>> GetOfferVehiculeSearch(string categori, string town, string searchOrAskJob, int price, string vehiculeRubrique, string vehiculeBrand, string vehiculeModel, string vehiculeType, string petrol, string year, string mileage, string numberOfDoor, string gearBox, string vehiculestate, string color, int pageIndex, int pageSize, string sortBy)
        {
            List<SearchViewModel> results = new List<SearchViewModel>();
            switch (sortBy)
            {
                case "MostRecent":
                    results = await db.Vehicules.OrderByDescending(o => o.id).
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
                  VehiculeBrand = s.BrandVehicule,
                  VehiculeModel = s.ModelVehicule,
                  VehiculeType = s.TypeVehicule,
                  Petrol = s.PetrolVehicule,
                  Year = s.YearVehicule,
                  Mileage = s.MileageVehicule,
                  NumberOfDoor = s.NumberOfDoorVehicule,
                  GearBox = s.GearBoxVehicule,
                  Color = s.ColorVehicule,
                  Vehiculestate = s.StateVehicule,
                  VehiculeRubrique = s.RubriqueVehicule

              }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                case "MostOld":
                    results = await db.Vehicules.OrderBy(o => o.id).
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
                  VehiculeBrand = s.BrandVehicule,
                  VehiculeModel = s.ModelVehicule,
                  VehiculeType = s.TypeVehicule,
                  Petrol = s.PetrolVehicule,
                  Year = s.YearVehicule,
                  Mileage = s.MileageVehicule,
                  NumberOfDoor = s.NumberOfDoorVehicule,
                  GearBox = s.GearBoxVehicule,
                  Color = s.ColorVehicule,
                  Vehiculestate = s.StateVehicule,
                  VehiculeRubrique = s.RubriqueVehicule

              }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                case "LowerPrice":

                    results = await db.Vehicules.OrderBy(o => o.Price).
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
                  VehiculeBrand = s.BrandVehicule,
                  VehiculeModel = s.ModelVehicule,
                  VehiculeType = s.TypeVehicule,
                  Petrol = s.PetrolVehicule,
                  Year = s.YearVehicule,
                  Mileage = s.MileageVehicule,
                  NumberOfDoor = s.NumberOfDoorVehicule,
                  GearBox = s.GearBoxVehicule,
                  Color = s.ColorVehicule,
                  Vehiculestate = s.StateVehicule,
                  VehiculeRubrique = s.RubriqueVehicule

              }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                case "HeigherPrice":

                    results = await db.Vehicules.OrderByDescending(o => o.Price).
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
                  VehiculeBrand = s.BrandVehicule,
                  VehiculeModel = s.ModelVehicule,
                  VehiculeType = s.TypeVehicule,
                  Petrol = s.PetrolVehicule,
                  Year = s.YearVehicule,
                  Mileage = s.MileageVehicule,
                  NumberOfDoor = s.NumberOfDoorVehicule,
                  GearBox = s.GearBoxVehicule,
                  Color = s.ColorVehicule,
                  Vehiculestate = s.StateVehicule,
                  VehiculeRubrique = s.RubriqueVehicule

              }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
                default:

                    results = await db.Vehicules.OrderByDescending(o =>o.id).
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
                  VehiculeBrand = s.BrandVehicule,
                  VehiculeModel = s.ModelVehicule,
                  VehiculeType = s.TypeVehicule,
                  Petrol = s.PetrolVehicule,
                  Year = s.YearVehicule,
                  Mileage = s.MileageVehicule,
                  NumberOfDoor = s.NumberOfDoorVehicule,
                  GearBox = s.GearBoxVehicule,
                  Color = s.ColorVehicule,
                  Vehiculestate = s.StateVehicule,
                  VehiculeRubrique = s.RubriqueVehicule

              }).Where(m => m.Category == categori && m.SearchOrAskJob == searchOrAskJob).ToListAsync();
                    break;
            }

             

            if (!string.IsNullOrWhiteSpace(vehiculeRubrique) && vehiculeRubrique != "Toutes")
            {
                results = results.Where(m => m.VehiculeRubrique == vehiculeRubrique).ToList();
            }
            if (!string.IsNullOrWhiteSpace(town) && town != "Toutes")
            {
                results = results.Where(m => m.Town == town).ToList();
            }
            if (price >= 0 && price < 2000000)
            {
                results = results.Where(m => m.Price <= price).ToList();
            }

            if (!string.IsNullOrWhiteSpace(year))
            {
                results = results.Where(m => Convert.ToInt32(m.Year) <= Convert.ToInt32(year)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(mileage))
            {
                results = results.Where(m => Convert.ToInt32(m.Mileage) <= Convert.ToInt32(mileage)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(vehiculeBrand) && vehiculeBrand != "Toutes")
            {
                results = results.Where(m => m.VehiculeBrand == vehiculeBrand).ToList();
            }

            if (!string.IsNullOrWhiteSpace(vehiculeModel) && vehiculeModel != "Tout")
            {
                results = results.Where(m => m.VehiculeModel == vehiculeModel).ToList();
            }

            if (!string.IsNullOrWhiteSpace(vehiculeType) && vehiculeType != "Tout")
            {
                results = results.Where(m => m.VehiculeType == vehiculeType).ToList();
            }

            if (!string.IsNullOrWhiteSpace(petrol) && petrol != "Tout")
            {
                results = results.Where(m => m.Petrol == petrol).ToList();
            }

            if (!string.IsNullOrWhiteSpace(numberOfDoor) && numberOfDoor != "Toutes")
            {
                results = results.Where(m => m.NumberOfDoor == numberOfDoor).ToList();
            }

            if (!string.IsNullOrWhiteSpace(gearBox) && gearBox != "Toutes")
            {
                results = results.Where(m => m.GearBox == gearBox).ToList();
            }
            if (!string.IsNullOrWhiteSpace(color) && color != "Toutes")
            {
                results = results.Where(m => m.Color == color).ToList();
            }
            if (!string.IsNullOrWhiteSpace(vehiculestate) && vehiculestate != "Tout")
            {
                results = results.Where(m => m.Vehiculestate == vehiculestate).ToList();
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

        // PUT: api/Vehicule/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVehiculeModel(int id, VehiculeCritereViewModel vehiculeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vehiculeModel.id)
            {
                return BadRequest();
            }

            var vehicule = await db.Vehicules.FirstOrDefaultAsync(m => m.id == id);


            vehicule.Price = vehiculeModel.Price;
            vehicule.SearchOrAskJob = vehiculeModel.SearchOrAsk;
            vehicule.RubriqueVehicule = vehiculeModel.RubriqueVehicule;
            vehicule.TypeVehicule = vehiculeModel.TypeVehicule;
            vehicule.BrandVehicule = vehiculeModel.BrandVehicule;
            vehicule.PetrolVehicule = vehiculeModel.PetrolVehicule;
            vehicule.ModelVehicule = vehiculeModel.ModelVehicule;
            vehicule.YearVehicule = vehiculeModel.YearVehicule;
            vehicule.FirstYearVehicule = vehiculeModel.FirstYearVehicule;
            vehicule.GearBoxVehicule = vehiculeModel.GearBoxVehicule;
            vehicule.MileageVehicule = vehiculeModel.MileageVehicule;
            vehicule.NumberOfDoorVehicule = vehiculeModel.NumberOfDoorVehicule;
            vehicule.ColorVehicule = vehiculeModel.ColorVehicule;
            vehicule.StateVehicule = vehiculeModel.StateVehicule;



            db.Entry(vehicule).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehiculeModelExists(id))
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

        // POST: api/Vehicule
        [ResponseType(typeof(VehiculeModel))]
        public async Task<IHttpActionResult> PostVehiculeModel(VehiculeModel vehiculeModel)
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
            vehiculeModel.User = user;
            vehiculeModel.DateAdd = DateTime.UtcNow;
            if (vehiculeModel.Coordinate == null || (vehiculeModel.Coordinate != null &&
               (vehiculeModel.Coordinate.Lat == null || vehiculeModel.Coordinate.Lon == null)))
            {
                vehiculeModel.Coordinate = await CoordonateService.GetCoodinateAsync(vehiculeModel.Town, vehiculeModel.Street);
            }

            vehiculeModel.Images = img;

          
            db.Vehicules.Add(vehiculeModel);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = vehiculeModel.id }, vehiculeModel);
        }

        // DELETE: api/Vehicule/5
        [ResponseType(typeof(VehiculeModel))]
        public async Task<IHttpActionResult> DeleteVehiculeModel(int id)
        {
            VehiculeModel vehiculeModel = await db.Vehicules.FindAsync(id);
            if (vehiculeModel == null)
            {
                return NotFound();
            }

            db.Products.Remove(vehiculeModel);
            await db.SaveChangesAsync();

            return Ok(vehiculeModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VehiculeModelExists(int id)
        {
            return db.Products.Count(e => e.id == id) > 0;
        }
    }
}