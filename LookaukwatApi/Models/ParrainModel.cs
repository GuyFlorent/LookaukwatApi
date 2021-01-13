using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LookaukwatApi.Models
{
    public class ParrainModel
    {
        public int Id { get; set; }
        [DisplayName("Date de création de l'agent")]
        public DateTime Date_createParrain { get; set; }
        public string ParrainEmail { get; set; }
        public string ParrainFirstName { get; set; }
    }
}