using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LookaukwatApi.Models
{
    public class ModeModel : ProductModel
    {
        [DisplayName("Rubrique")]
        public string RubriqueMode { get; set; }
        [DisplayName("Type")]
        public string TypeMode { get; set; }
        [DisplayName("Marque")]
        public string BrandMode { get; set; }
        [DisplayName("Univers")]
        public string UniversMode { get; set; }
        [DisplayName("Taille")]
        public string SizeMode { get; set; }

        [DisplayName("Couleur")]
        public string ColorMode { get; set; }
        [DisplayName("Etat")]
        public string StateMode { get; set; }
    }
}