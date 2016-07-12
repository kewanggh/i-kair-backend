using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.API.Models
{
    public class TransactionProduct
    {
        public int ProductId { get; set; }
        public int TransactionId { get; set; }

        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual Transaction Transaction { get; set; }

    }
}