using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LookaukwatApi.ViewModel
{
    public class JobCritereViewModel
    {
        public int id { get; set; }
       
        public int Price { get; set; }
  
        public string SearchOrAskJob { get; set; }

        public string TypeContract { get; set; }
        
        public string ActivitySector { get; set; }
    }
}