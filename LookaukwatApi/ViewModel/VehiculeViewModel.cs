using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LookaukwatApi.ViewModel
{
    public class VehiculeViewModel
    {

        public int id { get; set; }
        public int ViewNumber { get; set; }
        [DisplayName("Je vends/Je recherche")]
        public string SearchOrAsk { get; set; }
        [DisplayName("Ville")]
        public string Town { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "seule les valeurs positives sont acceptées")]
        [DisplayName("Prix(CFA)")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Le titre de l'annonce ne doit pas être vide")]

        [DisplayName("Titre de l'annonce*")]
        [StringLength(50)]
        public string Title { get; set; }
        [Required(ErrorMessage = "La Description de l'annonce ne doit pas être vide")]
        [DisplayName("Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Le Quartier de l'annonce ne doit pas être vide")]
        [DisplayName("Quartier")]
        public string Street { get; set; }
        public DateTime DateAdd { get; set; }
        //can change here
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
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public List<string> Images { get; set; }
        public List<SimilarProductViewModel> SimilarProduct { get; set; }
    }
}