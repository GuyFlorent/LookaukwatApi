using LookaukwatApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LookaukwatApi.ViewModel
{
    public class ModeViewModel
    {

        public int id { get; set; }
        public int NumberImages { get; set; }
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
        [DisplayName("Rubrique")]
        public string Rubrique { get; set; }
        [DisplayName("Type")]
        public DateTime DateAdd { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        [DisplayName("Marque")]
        public string Brand { get; set; }
       
        [DisplayName("Sexe")]
        public string Univers { get; set; }
        [DisplayName("Taille")]
        public string Size { get; set; }
        [DisplayName("Couleur")]
        public string Color { get; set; }
        [DisplayName("Etat")]
        public string State { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public int ViewNumber { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public List<string> Images { get; set; }
        public List<SimilarProductViewModel> SimilarProduct { get; set; }
        public bool IsLookaukwat { get; set; }
        public int Stock { get; set; }
    }
}