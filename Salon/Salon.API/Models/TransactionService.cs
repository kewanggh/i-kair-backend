using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.API.Models
{
    public class TransactionService
    {
        public int ServiceId { get; set; }
        public int TransactionId { get; set; }

        public int Quanitity { get; set; }

        public virtual Service Service { get; set; }
        public virtual Transaction Transaction { get; set; }

    }
}