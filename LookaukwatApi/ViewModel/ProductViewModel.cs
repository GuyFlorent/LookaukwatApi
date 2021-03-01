using LookaukwatApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LookaukwatApi.ViewModel
{
    public class ProductViewModel
    {
        public int id { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
       
        public string Town { get; set; }
      
        public string Street { get; set; }
        
        public int Price { get; set; }

        public virtual ProductCoordinateModel Coordinate { get; set; }
        public bool IsLookaukwat { get; set; }
    }
}