using LookaukwatApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LookaukwatApi.ViewModel
{
    public class JobViewModel
    {
        public int id { get; set; }
        [DisplayName("Titre")]
      
        public string Title { get; set; }
      
        public string Description { get; set; }
        [DisplayName("Ville")]
        public string Town { get; set; }
        [DisplayName("Quartier")]
      
        public string Street { get; set; }
        [DisplayName("Prix")]
        public int Price { get; set; }
        [DisplayName("Date d'ajout")]
        public DateTime DateAdd { get; set; }
        public string Date { get; set; }
        [DisplayName("J'offre/Je recherche")]
        public string SearchOrAskJob { get; set; }
        [DisplayName("Nombre de vues")]
        public int ViewNumber { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public virtual CategoryModel Category { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ProductCoordinateModel Coordinate { get; set; }
        public virtual List<ImageProcductModel> Images { get; set; }

        [DisplayName("Type de contrat")]
        public string TypeContract { get; set; }
        [DisplayName("Secteur d'activité")]
        public string ActivitySector { get; set; }
        public List<SimilarProductViewModel> SimilarProduct { get; set; }
    }
}