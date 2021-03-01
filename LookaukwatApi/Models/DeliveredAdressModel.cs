using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LookaukwatApi.Models
{
    public class DeliveredAdressModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Telephone { get; set; }
    }
}