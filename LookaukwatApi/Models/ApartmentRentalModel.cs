using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LookaukwatApi.Models
{
    public class ApartmentRentalModel : ProductModel
    {
        [DisplayName("Superficie")]
        public int ApartSurface { get; set; }
        [DisplayName("Nombre de pièce")]
        public int RoomNumber { get; set; }
        [DisplayName("Meublé ou non meublé")]
        public string FurnitureOrNot { get; set; }
        public string Type { get; set; }

    }
}