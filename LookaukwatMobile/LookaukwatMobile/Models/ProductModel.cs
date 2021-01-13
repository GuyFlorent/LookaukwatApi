using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Extended;

namespace LookaukwatMobile.Models
{
    public class ProductModel 
    {
        public int id { get; set; }
       
        public string Title { get; set; }
        public string Description { get; set; }
       
        public string Town { get; set; }
        
        public string Street { get; set; }
       
        public int Price { get; set; }
       
        public DateTime DateAdd { get; set; }
       
        public string SearchOrAskJob { get; set; }
       
        public int ViewNumber { get; set; }
        public virtual CategoryModel Category { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ProductCoordinateModel Coordinate { get; set; }
        public virtual List<ImageProcductModel> Images { get; set; }
    }
}
