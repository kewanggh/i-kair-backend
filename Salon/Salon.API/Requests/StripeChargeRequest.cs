using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.API.Requests
{
    public class StripeChargeRequest
    {
        public int TransactionId { get; set; }
        public string StripeToken { get; set; }
    }
}