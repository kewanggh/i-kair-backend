using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.API.DTO
{
    public class StylistDTO
    {
        public int StylistId { get; set; }

        // Fields relevant to a stylist
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        //public bool InSession
        //{
        //    get
        //    {
        //        return Appointments.Any(a => a.CheckinTime.HasValue && a.CheckoutTime == null);
        //    }
        //}
        //public IEnumerable<AppointmentDTO> Appointments { get; set; }
    }
}