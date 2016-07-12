using Salon.API.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.API.Models
{
    public class Product
    {
        
        // Primary Key
        public int ProductId { get; set; }
        

        // Fields relevant to a product
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }
        public int LikeCount { get; set; }
        public int Quantity { get; set; }
        // trying to add image
        public string ImageUrl { get; set;}

        //public int UserId { get; set; }
        //public virtual SalonUser User { get; set; }
        //public int PurchaseId { get; set; }
        //public virtual Purchase Purchase { get; set; }

        // Relationship fields
        public string UserId { get; set; }
        public virtual SalonUser User { get; set; }
        //public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<TransactionProduct> TransactionProducts { get; set; }
        public Product()
        {
        }

        public Product(ProductDTO model)
        {
            this.Update(model);
        }

        public void Update(ProductDTO model)
        {
            ProductId = model.ProductId;
            Name = model.Name;
            UnitPrice = model.UnitPrice;
            Description = model.Description;
            LikeCount = model.LikeCount;
            Quantity = model.Quantity;
            ImageUrl = model.ImageUrl;
           

        }
    }
}