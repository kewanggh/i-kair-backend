using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.API.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }


        // Fields relevant to a product
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }
        public int LikeCount { get; set; }
        public int Quantity { get; set; }
        // trying to add image
        public string ImageUrl { get; set; }
    }
}