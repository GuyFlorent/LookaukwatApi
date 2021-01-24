using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LookaukwatApi.ViewModel
{
    public class HouseCritereViewModel
    {
        public int id { get; set; }
       
        public string SearchOrAskJob { get; set; }
        public int Price { get; set; }
        public string RubriqueHouse { get; set; }
        
        public string TypeHouse { get; set; }
       
        public string FabricMaterialeHouse { get; set; }
        
        public string ColorHouse { get; set; }
       
        public string StateHouse { get; set; }
    }
}