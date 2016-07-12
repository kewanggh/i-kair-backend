using Microsoft.AspNet.Identity.EntityFramework;
using Salon.API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Salon.API.Infrastructure
{
    public class SalonDataContext : IdentityDbContext<SalonUser>
    {
        public SalonDataContext() : base("Salon")
        {

        }
        public IDbSet<Appointment> Appointments { get; set; }
        public IDbSet<Customer> Customers { get; set; }
        public IDbSet<Product> Products { get; set; }
        public IDbSet<Transaction> Transactions { get; set; }
        public IDbSet<Service> Services { get; set; }
        public IDbSet<Stylist> Stylists { get; set; }
        public IDbSet<TransactionProduct> TransactionProducts { get; set; }
        public IDbSet<TransactionService> TransactionServices { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SalonUser>()
                .HasMany(s => s.Appointments)
                .WithRequired(a => a.User)
                .HasForeignKey(a => a.UserId);

            // Configures Transactions to Products
            modelBuilder.Entity<TransactionProduct>()
                .HasKey(tp => new { tp.TransactionId, tp.ProductId });

            modelBuilder.Entity<Product>()
                .HasMany(p => p.TransactionProducts)
                .WithRequired(tp => tp.Product)
                .HasForeignKey(tp => tp.ProductId);

            modelBuilder.Entity<Transaction>()
                .HasMany(t => t.TransactionProducts)
                .WithRequired(tp => tp.Transaction)
                .HasForeignKey(tp => tp.TransactionId);


            // Configures Transactions to Services 
            modelBuilder.Entity<TransactionService>()
                .HasKey(ts => new { ts.TransactionId, ts.ServiceId });

            modelBuilder.Entity<Service>()
                .HasMany(s => s.TransactionServices)
                .WithRequired(ts => ts.Service)
                .HasForeignKey(ts => ts.ServiceId);

            modelBuilder.Entity<Transaction>()
               .HasMany(t => t.TransactionServices)
               .WithRequired(ts => ts.Transaction)
               .HasForeignKey(ts => ts.TransactionId);

            ////////////////////////////////////////
            // Configures Appointments to Transaction
            modelBuilder.Entity<Appointment>()
                .HasKey(a => a.AppointmentId)
                .HasMany(a => a.Transactions)
                .WithRequired(t => t.Appointment)
                .HasForeignKey(t => t.AppointmentId)
                .WillCascadeOnDelete(false);

            // Configures Customer to Appointments
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.CustomerId)
                .HasMany(c => c.Appointments)
                .WithRequired(a => a.Customer)
                .HasForeignKey(a => a.CustomerId);

            // Configures Customer to Transactions
            modelBuilder.Entity<Customer>()
               .HasMany(c => c.Transactions)
               .WithRequired(t => t.Customer)
               .HasForeignKey(t => t.CustomerId);

            // Configures Stylist to Appointments
            modelBuilder.Entity<Stylist>()
                .HasKey(s => s.StylistId)
                .HasMany(s => s.Appointments)
                .WithRequired(a => a.Stylist)
                .HasForeignKey(a => a.StylistId);


            base.OnModelCreating(modelBuilder);
        }


    }
}