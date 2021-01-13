using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LookaukwatApi.Models
{
    public class MultimediaModel : ProductModel
    {
        [DisplayName("Rubrique")]
        public string Type { get; set; }
        [DisplayName("Marque")]
        public string Brand { get; set; }
        [DisplayName("Modèle")]
        public string Model { get; set; }
        [DisplayName("Capacité de stockage")]
        public string Capacity { get; set; }
    }
}