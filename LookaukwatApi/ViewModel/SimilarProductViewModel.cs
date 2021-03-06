﻿using LookaukwatApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LookaukwatApi.ViewModel
{
    public class SimilarProductViewModel
    {
        public int id { get; set; }
        public string Title { get; set; }

        public string Town { get; set; }
       
        public int Price { get; set; }
     
        public DateTime DateAdd { get; set; }
        public string Date { get => ConvertDate.Convert(DateAdd); }

        public string  Category { get; set; }
        public bool IsLookaukwat { get; set; }
        public  string Image { get; set; }
        public  int NumberImages { get; set; }

    }
}