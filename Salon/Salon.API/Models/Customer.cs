using Salon.API.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.API.Models
{
    public class Customer
    {
       

        // Primary Key
        public int CustomerId { get; set; }
        

        // Fields relevant to a Customer
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Text { get; set; }

        public string StripeCustomerId { get; set; }


        // Relationship fields
        public string UserId { get; set; }
        public virtual SalonUser User { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public Customer()
        {
        }

        public Customer(CustomerDTO model)
        {
            this.Update(model);
        }

        public void Update(CustomerDTO model)
        {
            CustomerId = model.CustomerId;
            FirstName = model.FirstName;
            LastName = model.LastName;
            Phone = model.Phone;
            Email = model.Email;
            Gender = model.Gender;
            Text = model.Text;
            StripeCustomerId = model.StripeCustomerId;

        }

    }
}