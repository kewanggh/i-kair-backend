using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.API.DTO
{
    public class ServiceDTO
    {
        public int ServiceId { get; set; }

        public string Name { get; set; }

        public decimal UnitPrice { get; set; }

        public string Description { get; set; }
    }
}