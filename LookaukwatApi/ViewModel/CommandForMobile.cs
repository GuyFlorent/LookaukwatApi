using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LookaukwatApi.ViewModel
{
    public class CommandForMobile
    {
        public int Id { get; set; }
        public int CommandId { get; set; }
        public DateTime? CommandDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsHomeDelivered { get; set; }
    }
}