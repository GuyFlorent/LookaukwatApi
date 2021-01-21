using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LookaukwatApi.ViewModel
{
    public class ImageModelView
    {
        public Guid id { get; set; }
        public string Image { get; set; }
        public string ImageMobile { get; set; }
        public List<HttpPostedFileBase> ImageFile { get; set; }
    }
}