using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LookaukwatApi.Models
{
    public class HouseModel : ProductModel
    {
        [DisplayName("Rubrique")]
        public string RubriqueHouse { get; set; }
        [DisplayName("Type")]
        public string TypeHouse { get; set; }
        [DisplayName("Matière")]
        public string FabricMaterialeHouse { get; set; }
        [DisplayName("Couleur")]
        public string ColorHouse { get; set; }
        [DisplayName("état")]
        public string StateHouse { get; set; }
    }
}