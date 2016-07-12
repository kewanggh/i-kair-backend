using Salon.API.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.API.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public DateTime Date { get; set; }
        public decimal? TotalAmount { get; set; }
        public string ChargeId { get; set; }  

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public string UserId { get; set; }
        public virtual SalonUser User { get; set; }
        //public virtual ICollection<Product> Products { get; set; }
        
        //public bool isPaid
        //{
        //    get
        //    {
        //        if (ChargeId == null)
        //        {
        //            //return ChargeId.Length==0;
        //            return false;
        //        }
        //        else {
        //            return false;
        //        }
        //    }
        //}  

        public int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }
        public virtual ICollection<TransactionProduct> TransactionProducts { get; set; }
        public virtual ICollection<TransactionService> TransactionServices { get; set; }
        public Transaction()
        {
        }

        public Transaction(TransactionDTO model)
        {
            this.Update(model);
        }

        public void Update(TransactionDTO model)
        {
            TransactionId = model.TransactionId;
            Date = model.Date;
            TotalAmount = model.TotalAmount;
            ChargeId = model.ChargeId;
            CustomerId = model.CustomerId;
            AppointmentId = model.AppointmentId;

        }
    }
}