using System;
using System.Collections.Generic;
using System.Text;

namespace LookaukwatMobile.ViewModels
{
    public class ProductForMobileViewModel
    {
        public int id { get; set; }

        public string Title { get; set; }


        public string Town { get; set; }


        public int Price { get; set; }

        public DateTime DateAdd { get; set; }
        public string Image { get; set; }
    }
}
