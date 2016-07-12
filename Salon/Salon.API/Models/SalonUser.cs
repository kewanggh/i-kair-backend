using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.API.Models
{
    public class SalonUser : IdentityUser
    {
       [Required]
       [MaxLength(100)]
        public string SalonName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        //
        //[Required]
        //public byte Level { get; set; }
        //
        //[Required]
        //public DateTime JoinDate { get; set; }

        // Setup our relationships
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Stylist> Stylists { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Service> Services { get; set; }


    }
}