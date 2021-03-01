using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LookaukwatApi.ViewModel
{
    public class TrackingCommandViewModel
    {
        public DateTime Date { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string Road { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
       
    }
}