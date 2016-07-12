using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.API.DTO
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }


        // Fields relevant to a Customer
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Text { get; set; }
        public int AppointmentsCount { get; set; }

        public string StripeCustomerId { get; set; }
        //public List<AppointmentDTO> Appointments { get; set; }
        //public List<TransactionDTO> Transactions { get; set; }
      
    }
}