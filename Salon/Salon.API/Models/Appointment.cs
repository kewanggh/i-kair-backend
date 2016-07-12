using Salon.API.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Salon.API.Models
{
    public class Appointment
    {
        //public Appointment()
        //{
          //  Services = new Collection<Service>();
        //}

        // Primary Keys
        public int AppointmentId { get; set; }


        // Fields relevant to a Appointment
        //public static int ReminderTime = 30;
        public DateTime CreatedDate { get; set; }
        public string Text { get; set; }
        public DateTime ScheduleCheckin { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public bool Completed
        {
            get
            {
                return CheckinTime.HasValue && CheckoutTime.HasValue;
            }
        }
        public bool ReminderSmsSent { get; set; }

        // Foreign Keys
        // Relationship fields
        public int CustomerId { get; set; }
        public int StylistId { get; set; }
        public virtual Customer Customer { get; set;}
        public virtual Stylist Stylist { get; set;}
        public string UserId { get; set; }
        public virtual SalonUser User { get; set; }
        
        //public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<Transaction> Transactions {get; set;}
        public Appointment()
        {
        }

        public Appointment(AppointmentDTO model)
        {
            this.Update(model);
        }

        public void Update(AppointmentDTO model)
        {
            AppointmentId = model.AppointmentId;
            CustomerId = model.CustomerId;
            StylistId = model.StylistId;
            CreatedDate = model.CreatedDate;
            Text = model.Text;
            ScheduleCheckin = model.ScheduleCheckin;
            CheckinTime = model.CheckinTime;
            CheckoutTime = model.CheckoutTime;

        }



    }
}