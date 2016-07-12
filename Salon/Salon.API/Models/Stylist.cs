using Salon.API.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Salon.API.Models
{
    public class Stylist
    {
        public Stylist()
        {
            Appointments = new Collection<Appointment>();
        }

        // Primary key
        public int StylistId { get; set; }

        // Fields relevant to a stylist
        public string FirstName { get; set; }
        public string LastName { get; set;}
        public string Email { get; set; }
        public string Description { get; set; }
        //public bool InSession
        //{
        //    get
        //    {
        //        return Appointments.Any(a => a.CheckinTime.HasValue && a.CheckoutTime == null);
        //    }
        //}


        // Relationship fields
        public string UserId { get; set; }
        public virtual SalonUser User { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
      

        public Stylist(StylistDTO model)
        {
            this.Update(model);
        }

        public void Update(StylistDTO model)
        {
            StylistId = model.StylistId;
            FirstName = model.FirstName;
            LastName = model.LastName;
           
            Email = model.Email;
            Description = model.Description;

        }

    }
}