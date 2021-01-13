﻿using System;
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
using LookaukwatApi.Models;
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


            return r;
        }

        [Route("api/Product/GetProductScrollView")]
        public List<ProductForMobile> GetScrollViewProducts(int pageIndex, int pageSize)
        {
            //var t = db.Products.ToList();
            var r = db.Products.OrderByDescending(o => o.id).Skip(pageIndex * pageSize).Take(pageSize).Select(s => new ProductForMobile
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


            return r;
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

        public async Task <string> PostUploadImagesProducts(int id)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var product = await db.Products.FindAsync(id);
                    string ImagePath = null;
                    if(product != null)
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
                            ImageMobile = "https://lookaukwat.azurewebsites.net/" + Image,
                            id = Guid.NewGuid(),
                            ProductId = id
                        };
                            product.Images.Add(picture);

                        FileName = Path.Combine(path, FileName);

                        poestedFile.SaveAs(FileName);

                            ImagePath = Image;
                        
                    }

                   await db.SaveChangesAsync();

                    return ImagePath;
                }
            }catch(Exception e) { return e.Message; }

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
          
            if(!string.IsNullOrWhiteSpace(categori) && categori != "Toutes les Catégories")
            {
                results = results.Where(m => m.Category == categori).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town))
            {
                results = results.Where(m => m.Town == town).ToList();
            }


            return results.Count;
        }


        // Result list of product of Ask and offer search
        [Route("api/Product/GetAskAndOfferSearch")]
        public async Task<List<ProductForMobile>> GetAskAndOfferSearch(string categori, string town, string searchOrAsk, int pageIndex, int pageSize)
        {

            var results = await db.Products.
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

            if (!string.IsNullOrWhiteSpace(categori) && categori != "Toutes les Catégories")
            {
                results = results.Where(m => m.Category == categori).ToList();
            }

            if (!string.IsNullOrWhiteSpace(town))
            {
                results = results.Where(m => m.Town == town).ToList();
            }


            return results.OrderByDescending(o => o.id).Skip(pageIndex * pageSize).Take(pageSize).ToList();
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

        [Route("api/Product/GetUserName")]
        public async Task<string> GetUserName(string phone)
        {
           var user = await db.Users.FirstOrDefaultAsync(e => e.PhoneNumber == phone);
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
        public async Task<IHttpActionResult> PutProductModel(int id, ProductModel productModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productModel.id)
            {
                return BadRequest();
            }

            db.Entry(productModel).State = EntityState.Modified;

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
        [Authorize]
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
                    path = item.Image;
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