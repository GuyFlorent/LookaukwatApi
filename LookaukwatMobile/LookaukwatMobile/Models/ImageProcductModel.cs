using System;
using System.Collections.Generic;
using System.Text;

namespace LookaukwatMobile.Models
{
    public class ImageProcductModel
    {
        public Guid id { get; set; }
        public string Image { get; set; }
        public string ImageMobile { get; set; }

        public int ProductId { get; set; }
        public ProductModel Product { get; set; }
    }
}
