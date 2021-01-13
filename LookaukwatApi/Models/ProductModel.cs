using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LookaukwatApi.Models
{
    public class ProductModel
    {
        
        public int id { get; set; }
        [DisplayName("Titre")]
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [DisplayName("Ville")]
        public string Town { get; set; }
        [DisplayName("Quartier")]
        [Required]
        public string Street { get; set; }
        [DisplayName("Prix")]
        public int Price { get; set; }
        [DisplayName("Date d'ajout")]
        public DateTime DateAdd { get; set; }
        [DisplayName("J'offre/Je recherche")]
        public string SearchOrAskJob { get; set; }
        [DisplayName("Nombre de vues")]
        public int ViewNumber { get; set; }
        public virtual CategoryModel Category { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ProductCoordinateModel Coordinate { get; set; }
        public virtual List<ImageProcductModel> Images { get; set; }
    }
}