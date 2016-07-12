using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.API.DTO
{
    public class AppointmentDTO
    {
        public int AppointmentId { get; set; }

        public DateTime CreatedDate { get; set; }
        public string Text { get; set; }
        public DateTime ScheduleCheckin { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
     
        public bool ReminderSmsSent { get; set; }
        public int CustomerId { get; set; }
        public int StylistId { get; set; }
        public CustomerDTO Customer { get; set; }
        public StylistDTO Stylist { get; set; }
       
    }
}