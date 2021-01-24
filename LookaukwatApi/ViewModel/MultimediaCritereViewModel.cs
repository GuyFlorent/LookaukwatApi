using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LookaukwatApi.ViewModel
{
    public class MultimediaCritereViewModel
    {
        public int id { get; set; }
       
        public string SearchOrAsk { get; set; }
       
        public int Price { get; set; }
        
        public string Type { get; set; }
       
        public string Brand { get; set; }
       
        public string Model { get; set; }

        public string Capacity { get; set; }
    }
}