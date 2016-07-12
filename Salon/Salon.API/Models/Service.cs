using Salon.API.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.API.Models
{
    public class Service
    {
        public int ServiceId { get; set; }

        public string Name { get; set; }

        public decimal UnitPrice { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }
        public virtual SalonUser User { get; set; }
        //public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<TransactionService> TransactionServices { get; set; }
        public Service()
        {
        }

        public Service(ServiceDTO model)
        {
            this.Update(model);
        }

        public void Update(ServiceDTO model)
        {
            ServiceId = model.ServiceId;
            Name = model.Name;
            UnitPrice = model.UnitPrice;
            Description = model.Description;
          

        }
    }
}