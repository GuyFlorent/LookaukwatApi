using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LookaukwatApi.Models
{
    public class JobModel : ProductModel
    {
        [DisplayName("Type de contrat")]
        public string TypeContract { get; set; }
        [DisplayName("Secteur d'activité")]
        public string ActivitySector { get; set; }

    }
}