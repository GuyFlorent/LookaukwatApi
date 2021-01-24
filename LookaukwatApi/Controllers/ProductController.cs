using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.ModelBinding;
using LookaukwatApi.Models;
using LookaukwatApi.Services;
using LookaukwatApi.ViewModel;
using Microsoft.AspNet.Identity;

namespace LookaukwatApi.Controllers
{
    public class ProductController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Product
        public List<ProductForMobile> GetProducts()
        {
            //var t = db.Products.ToList();
            var r = db.Products.Select(s => new ProductForMobile
            {
                Title = s.Title,
                Town = s.Town,
                DateAdd = s.DateAdd,
                ViewNumber = s.ViewNumber,
                Category = s.Category.CategoryName,
                Image = s.Images.Select(im=>im.ImageMobile).FirstOrDefault(),
                id = s.id,
                Price = s.Price
            }).OrderByDescending(o=>o.id).ToList();


            return r;
        }

        [Authorize]
        [Route("api/Product/GetUserProducts")]
        public List<ProductForMobile> GetUserProducts()
        {
            string UserId = User.Identity.GetUserId();
           
            var r = db.Products.Where(m => m.User.Id == UserId).Select(s => new ProductForMobile
            {
                Title = s.Title,
                Town = s.Town,
                DateAdd = s.DateAdd,
                ViewNumber = s.ViewNumber,
                Category = s.Category.CategoryName,
                Image = s.Images.Select(im => im.ImageMobile).FirstOrDefault(),
                id = s.id,
                Price = s.Price
            }).OrderByDescending(o => o.id).ToList();

            List<ProductForMobile> Liste = new List<ProductForMobile>();

            foreach (var item in r)
            {
                item.Date = ConvertDate.Convert(item.DateAdd);
                Liste.Add(item);
            }

            return Liste;

          
        }

        [Route("api/Product/GetProductScrollView")]
        public List<ProductForMobile> GetScrollViewProducts(int pageIndex, int pageSize, string sortBy)
        {
            List<ProductForMobile> List = new List<ProductForMobile>();
            switch (sortBy)
            {
                case "MostRecent":
                    List = db.Products.OrderByDescending(o => o.id).Skip(pageIndex * pageSize).Take(pageSize).Select(s => new ProductForMobile
                    {
                        Title = s.Title,
                        Town = s.Town,
                        DateAdd = s.DateAdd,
                        ViewNumber = s.ViewNumber,
                        Category = s.Category.CategoryName,
                        Image = s.Images.Select(im => im.ImageMobile).FirstOrDefault(),
                        id = s.id,
                        Price = s.Price
                    }).ToList();
                    break;

                case "MostOld":
                    List = db.Products.OrderBy(o => o.id).Skip(pageIndex * pageSize).Take(pageSize).Select(s => new ProductForMobile
                    {
                        Title = s.Title,
                        Town = s.Town,
                        DateAdd = s.DateAdd,
                        ViewNumber = s.ViewNumber,
                        Category = s.Category.CategoryName,
                        Image = s.Images.Select(im => im.ImageMobile).FirstOrDefault(),
                        id = s.id,
                        Price = s.Price
                    }).ToList();
                    break;

                case "LowerPrice":
                    List = db.Products.OrderBy(o => o.Price).Skip(pageIndex * pageSize).Take(pageSize).Select(s => new ProductForMobile
                    {
                        Title = s.Title,
                        Town = s.Town,
                        DateAdd = s.DateAdd,
                        ViewNumber = s.ViewNumber,
                        Category = s.Category.CategoryName,
                        Image = s.Images.Select(im => im.ImageMobile).FirstOrDefault(),
                        id = s.id,
                        Price = s.Price
                    }).ToList();
                    break;

                case "HeigherPrice":
                    List = db.Products.OrderByDescending(o => o.Price).Skip(pageIndex * pageSize).Take(pageSize).Select(s => new ProductForMobile
                    {
                        Title = s.Title,
                        Town = s.Town,
                        DateAdd = s.DateAdd,
                        ViewNumber = s.ViewNumber,
                        Category = s.Category.CategoryName,
                        Image = s.Images.Select(im => im.ImageMobile).FirstOrDefault(),
                        id = s.id,
                        Price = s.Price
                    }).ToList();
                    break;
                default:
                    List = db.Products.OrderByDescending(o => o.id).Skip(pageIndex * pageSize).Take(pageSize).Select(s => new ProductForMobile
                    {
                        Title = s.Title,
                        Town = s.Town,
                        DateAdd = s.DateAdd,
                        ViewNumber = s.ViewNumber,
                        Category = s.Category.CategoryName,
                        Image = s.Images.Select(im => im.ImageMobile).FirstOrDefault(),
                        id = s.id,
                        Price = s.Price
                    }).ToList();

                    
                    break;
            }

            List<ProductForMobile> Liste = new List<ProductForMobile>();

            foreach(var item in List)
            {
                item.Date = ConvertDate.Convert(item.DateAdd);
                Liste.Add(item);
            }

            return Liste;
        }

        [Route("api/Product/GetSearchProducts")]
        public List<ProductForMobile> GetSearchProducts(string newValue)
        {
            //var t = db.Products.ToList();
            var r = db.Products.Where(m => (m.Title != null && m.Title.ToLower().Contains(newValue.ToLower()))
                || (m.Description != null && m.Description.ToLower().Contains(newValue.ToLower())) ||
               (m.Street != null && m.Street.ToLower().Contains(newValue.ToLower())) ||
               (m.SearchOrAskJob != null && m.SearchOrAskJob.ToLower().Contains(newValue.ToLower()))).OrderByDescending(o => o.id).Select(s => new ProductForMobile
            {
                Title = s.Title,
                Category = s.Category.CategoryName,
                Image = s.Images.Select(im => im.ImageMobile).FirstOrDefault(),
                id = s.id,
                
            }).ToList();


            return r;
        }

        [Route("api/Product/TotalNumberOfProduct")]
        public int GetTotalNumberProducts()
        {
           
            return db.Products.Count();
        }

        [Route("api/Product/UploadImages")]

        public async Task <ImageProcductModel> PostUploadImagesProducts(int id)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var product = await db.Products.FindAsync(id);
                    string ImagePath = null;
                    ImageProcductModel img = new ImageProcductModel();
                    if (product != null)
                    foreach (string file in httpRequest.Files)
                    {
                        var poestedFile = httpRequest.Files[file];
                        var FileName = Path.GetFileNameWithoutExtension(poestedFile.FileName);
                        string FileExtension = Path.GetExtension(poestedFile.FileName);
                        FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName + FileExtension;
                        var path = HttpContext.Current.Server.MapPath("~/UserImage/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        var Image = $"/UserImage/{FileName}";

                            ImageProcductModel picture = new ImageProcductModel
                            {
                            Image = Image,
                            ImageMobile = "https://lookaukwatapi.azurewebsites.net" + Image,
                            id = Guid.NewGuid(),
                            ProductId = id
                        };
                            product.Images.Add(picture);

                        FileName = Path.Combine(path, FileName);

                        poestedFile.SaveAs(FileName);

                            ImagePath = Image;
                            img = picture;
                    }

                   await db.SaveChangesAsync();

                    return img;
                }
            }catch(Exception e) { Console.WriteLine(e); }

            return null;
        }

        [Route("api/Product/UpdateProductImage")]
        public async Task<string> UpdateProductImage(int id)
        {
            ProductModel productModel = await db.Products.FindAsync(id);
            if (productModel == null)
            {
                return "Not found";
            }
            if(productModel.Images.Count > 1)
            {
                var image = productModel.Images.FirstOrDefault(m => m.Image.StartsWith("http"));
                if (image != null)
                    db.Images.Remove(image);
                await db.SaveChangesAsync();
            }
            

            return "Ok";
        }


        // Result of Ask and offer search
        [Route("api/Product/PostMessage")]
        public async Task<bool> PostMessage(contactUserViewModel contact)
        {
          var response =  await SendGridService.configSendGridasync(contact);
            return response;
        }


        // Result of Ask and offer search
        [Route("api/Product/GetAskAndOfferSearchNumber")]
        public async Task<int> GetAskAndOfferSearchNumber(string categori, string town, string searchOrAsk)
        {
            
              var results =   await db.Products.
                Select(s => new ProductForMobile
                {
                    Category = s.Category.CategoryName,
                    Town = s.Town,
                    SearchOrAsk = s.SearchOrAskJob
                }).Where(m =>  m.SearchOrAsk == searchOrAsk)
                .ToListAsync();
          
            if(!string.IsNullOrWhiteSpace(categori) && categori != "Toutes")
            {
                results = results.Where(m => m.Category == categori).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town) && town != "Toutes")
            {
                results = results.Where(m => m.Town == town).ToList();
            }


            return results.Count;
        }


        // Result list of product of Ask and offer search
        [Route("api/Product/GetAskAndOfferSearch")]
        public async Task<List<ProductForMobile>> GetAskAndOfferSearch(string categori, string town, string searchOrAsk, int pageIndex, int pageSize, string sortBy)
        {
            var results = new List<ProductForMobile>();

            switch (sortBy)
            {
                case "MostRecent":
                    results = await db.Products.OrderByDescending(o => o.id).
              Select(s => new ProductForMobile
              {
                  Title = s.Title,
                  Category = s.Category.CategoryName,
                  Town = s.Town,
                  SearchOrAsk = s.SearchOrAskJob,
                  Image = s.Images.Select(im => im.ImageMobile).FirstOrDefault(),
                  id = s.id,
                  DateAdd = s.DateAdd,
                  Price = s.Price
              }).Where(m => m.SearchOrAsk == searchOrAsk)
              .ToListAsync();
                    break;
                case "MostOld":
                    results = await db.Products.OrderBy(o => o.id).
              Select(s => new ProductForMobile
              {
                  Title = s.Title,
                  Category = s.Category.CategoryName,
                  Town = s.Town,
                  SearchOrAsk = s.SearchOrAskJob,
                  Image = s.Images.Select(im => im.ImageMobile).FirstOrDefault(),
                  id = s.id,
                  DateAdd = s.DateAdd,
                  Price = s.Price
              }).Where(m => m.SearchOrAsk == searchOrAsk)
              .ToListAsync();
                    break;
                case "LowerPrice":
                    results = await db.Products.OrderBy(o => o.Price).
              Select(s => new ProductForMobile
              {
                  Title = s.Title,
                  Category = s.Category.CategoryName,
                  Town = s.Town,
                  SearchOrAsk = s.SearchOrAskJob,
                  Image = s.Images.Select(im => im.ImageMobile).FirstOrDefault(),
                  id = s.id,
                  DateAdd = s.DateAdd,
                  Price = s.Price
              }).Where(m => m.SearchOrAsk == searchOrAsk)
              .ToListAsync();
                    break;
                case "HeigherPrice":
                    results = await db.Products.OrderByDescending(o => o.Price).
              Select(s => new ProductForMobile
              {
                  Title = s.Title,
                  Category = s.Category.CategoryName,
                  Town = s.Town,
                  SearchOrAsk = s.SearchOrAskJob,
                  Image = s.Images.Select(im => im.ImageMobile).FirstOrDefault(),
                  id = s.id,
                  DateAdd = s.DateAdd,
                  Price = s.Price
              }).Where(m => m.SearchOrAsk == searchOrAsk)
              .ToListAsync();
                    break;
                default:
                    results = await db.Products.OrderByDescending(o => o.id).
              Select(s => new ProductForMobile
              {
                  Title = s.Title,
                  Category = s.Category.CategoryName,
                  Town = s.Town,
                  SearchOrAsk = s.SearchOrAskJob,
                  Image = s.Images.Select(im => im.ImageMobile).FirstOrDefault(),
                  id = s.id,
                  DateAdd = s.DateAdd,
                  Price = s.Price
              }).Where(m => m.SearchOrAsk == searchOrAsk)
              .ToListAsync();
                    break;
            }
             

            if (!string.IsNullOrWhiteSpace(categori) && categori != "Toutes")
            {
                results = results.Where(m => m.Category == categori).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town) && town != "Toutes")
            {
                results = results.Where(m => m.Town == town).ToList();
            }

            List<ProductForMobile> Liste = new List<ProductForMobile>();

            foreach (var item in results)
            {
                item.Date = ConvertDate.Convert(item.DateAdd);
                Liste.Add(item);
            }

            return Liste.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }


        // GET: api//5
        [ResponseType(typeof(ProductModel))]
        public async Task<IHttpActionResult> GetProductModel(int id)
        {
            ProductModel productModel = await db.Products.FindAsync(id);
            if (productModel == null)
            {
                return NotFound();
            }

            return Ok(productModel);
        }


        // GET: api//5
        [ResponseType(typeof(ProductModel))]
        [Route("api/Product/GetProductWithSameParm")]
        public async Task<IHttpActionResult> GetProductSameParm(int id)
        {
            ProductModel productModel = await db.Products.FindAsync(id);
            if (productModel == null)
            {
                return NotFound();
            }
            ProductViewModel prod = new ProductViewModel()
            {
                id = productModel.id,
                Title = productModel.Title,
                Description = productModel.Description,
                Town = productModel.Town,
                Street = productModel.Street
            };
            return Ok(prod);
        }

        //For user checking
        [ResponseType(typeof(ApplicationUser))]
        [Authorize]
        [Route("api/Product/GetUserInformation")]
        public async Task<IHttpActionResult> GetUserInformation()
        {
            string UserId = User.Identity.GetUserId();
            
            ApplicationUser user = await db.Users.FirstOrDefaultAsync(u =>u.Id ==UserId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("api/Product/GetUserName")]
        public async Task<string> GetUserName(UserPhoneViewModel Phone)
        {
           var user = await db.Users.FirstOrDefaultAsync(e => e.PhoneNumber == Phone.phoneNumber);
            if (user == null)
                return null;
            return user.Email;
        }
        [Route("api/Product/CheckUserPhoneExists")]
        public async Task<bool> UserPhoneExists(ApplicationUser user)
        {
            return await db.Users.CountAsync(e => e.PhoneNumber == user.PhoneNumber) > 0;
        }

        [Route("api/Product/CheckUserEmailExists")]
        public async Task <bool> UserEmailExists(ApplicationUser user)
        {
            return await db.Users.CountAsync(e => e.Email == user.Email) > 0;
        }

        [Route("api/Product/UpdateUserInfo")]
        public async Task<bool> UpdateUserInfo(ApplicationUser user)
        {
            string UserId = User.Identity.GetUserId();
            var UpdateUser = db.Users.FirstOrDefault(m => m.Id == UserId);
            UpdateUser.FirstName = user.FirstName;
            UpdateUser.PhoneNumber = user.PhoneNumber;
            UpdateUser.Email = user.Email;
            UpdateUser.UserName = user.Email;
           var result =  await db.SaveChangesAsync();
            return true;
        }

        // PUT: api/Product/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProductModel(int id, ProductViewModel productModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productModel.id)
            {
                return BadRequest();
            }

            var prod = await db.Products.FirstOrDefaultAsync(m => m.id == id);


            prod.Title = productModel.Title;
            prod.Description = productModel.Description;
            prod.Town = productModel.Town;
            prod.Street = productModel.Street;

            if (productModel.Coordinate == null || (productModel.Coordinate != null &&
               (productModel.Coordinate.Lat == null || productModel.Coordinate.Lon == null)))
            {
                prod.Coordinate = await CoordonateService.GetCoodinateAsync(productModel.Town, productModel.Street);
            }
            else
            {
                prod.Coordinate = productModel.Coordinate;
            }
           


            db.Entry(prod).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductModelExists(id))
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

        // POST: api/Product
        [ResponseType(typeof(ProductModel))]
        public async Task<IHttpActionResult> PostProductModel(ProductModel productModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(productModel);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = productModel.id }, productModel);
        }

        // DELETE: api/Product/5
        [ResponseType(typeof(ProductModel))]
        //[Authorize]
        public async Task<IHttpActionResult> DeleteProductModel(int id)
        {
            ProductModel productModel = await db.Products.FindAsync(id);
            if (productModel == null)
            {
                return NotFound();
            }

            //delete files from the file system
            String path = null;
            foreach (var item in productModel.Images)
            {
                if (item.Image.StartsWith("http"))
                {
                    path = item.Image;
                }
                else
                {
                    path = System.Web.Hosting.HostingEnvironment.MapPath(item.Image);
                }

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            db.Products.Remove(productModel);
            await db.SaveChangesAsync();

            return Ok(productModel);
        }

        [HttpGet]
        [Route("api/Product/GetImages")]
        [Authorize]
        public async Task<List<ImageModelView>> GetImageList(int ProductId)
        {
            return await db.Images.Where(im => im.ProductId == ProductId).Select(s => new ImageModelView
            {
                id = s.id,
                ImageMobile = s.ImageMobile,
            }).ToListAsync();
        }

        [HttpDelete]
        [Route("api/Product/DeleteImages")]
        [ResponseType(typeof(ImageProcductModel))]
        [Authorize]
        public async Task<IHttpActionResult> DeleteImage(Guid ImageId)
        {
            var img = await db.Images.FirstOrDefaultAsync(m =>m.id == ImageId);

            if(img == null)
            {
                return NotFound();
            }

            string path;
            if (img.Image.StartsWith("http"))
            {
                path = img.Image;
            }
            else
            {
                path = System.Web.Hosting.HostingEnvironment.MapPath(img.Image);
            }

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

             db.Images.Remove(img);

            var result = await db.SaveChangesAsync();
            return Ok(img);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductModelExists(int id)
        {
            return db.Products.Count(e => e.id == id) > 0;
        }
    }
}