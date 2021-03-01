using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LookaukwatApi.Models
{
    public class VehiculeModel : ProductModel
    {
        [DisplayName("Rubrique")]
        public string RubriqueVehicule { get; set; }
        [DisplayName("Marque")]
        public string BrandVehicule { get; set; }
        [DisplayName("Modèle")]
        public string ModelVehicule { get; set; }
        [DisplayName("Type")]
        public string TypeVehicule { get; set; }
        [DisplayName("Carburant")]
        public string PetrolVehicule { get; set; }
        [DisplayName("Mise en circulation")]
        public string FirstYearVehicule { get; set; }
        [DisplayName("Année")]
        public string YearVehicule { get; set; }
        [DisplayName("Kilométrage")]
        public string MileageVehicule { get; set; }
        [DisplayName("Nombre de portes")]
        public string NumberOfDoorVehicule { get; set; }
        [DisplayName("Couleur")]
        public string ColorVehicule { get; set; }
        [DisplayName("Etat")]
        public string StateVehicule { get; set; }
        [DisplayName("Transmission")]
        public string GearBoxVehicule { get; set; }
       
        //[DisplayName("Modèle")]
        //public string ModelAccessoryAutoVehicule { get; set; }
        //[DisplayName("Modèle")]
        //public string ModelAccessoryBikeVehicule { get; set; }
    }
}