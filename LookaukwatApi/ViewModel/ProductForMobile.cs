using LookaukwatApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LookaukwatApi.ViewModel
{
    public class ProductForMobile
    {
        public int id { get; set; }
    
        public string Title { get; set; }
        public string SearchOrAsk { get; set; }
 
      
        public string Town { get; set; }
        public string Category { get; set; }
        public int ViewNumber { get; set; }
        public int CallNumber { get; set; }
        public int MessageNumber { get; set; }
        public bool IsLookaukwat { get; set; }
        public bool IsParticulier { get; set; }
        public bool IsActive { get; set; }
        public int Price { get; set; }
      
        public DateTime DateAdd { get; set; }
        public string Date { get; set; }
        public string DateLetter { get => ConvertDate.Convert(DateAdd); }
        public string Image { get; set; }
        public int NumberImages { get; set; }



    }
}