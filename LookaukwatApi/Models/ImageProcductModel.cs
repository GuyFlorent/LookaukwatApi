using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LookaukwatApi.Models
{
    public class ImageProcductModel
    {
        public Guid id { get; set; }
        public string Image { get; set; }
        public string ImageMobile { get; set; }
        // public List <HttpPostedFileBase> ImageFile { get; set; }
        public int ProductId { get; set; }
        public ProductModel Product { get; set; }
    }
}