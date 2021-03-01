using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LookaukwatApi.Models
{
    public class CommandModel
    {
        public int Id { get; set; }
        public int TotalPrice { get; set; }
        public int DeliveredPrice { get; set; }
        public string Distance { get; set; }
        public string PayementMethod { get; set; }
        public int CommandId { get; set; }
        public DateTime? CommandDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public bool IsHomeDelivered { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsDeleteByUser { get; set; }
        public virtual ApplicationUser user { get; set; }
        public virtual List<PurchaseModel> Purchases { get; set; }
        public virtual DeliveredAdressModel DeliveredAdress { get; set; }
    }
}