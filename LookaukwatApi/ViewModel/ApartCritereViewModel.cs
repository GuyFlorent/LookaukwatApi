using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LookaukwatApi.ViewModel
{
    public class ApartCritereViewModel
    {
        public int id { get; set; }
        public string SearchOrAsk { get; set; }

        public int Price { get; set; }

        public string Type { get; set; }

        public int ApartSurface { get; set; }

        public int RoomNumber { get; set; }

        public string FurnitureOrNot { get; set; }
    }
}