using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.API.DTO
{
    public class TransactionDTO
    {
        public int TransactionId { get; set; }

        public DateTime Date { get; set; }
        public decimal? TotalAmount { get; set; }
        public string ChargeId { get; set; }
 
        public int AppointmentId { get; set; }
        public int CustomerId { get; set; }
        public AppointmentDTO Appointment { get; set; }
        public CustomerDTO Customer { get; set; }
       
    }
}