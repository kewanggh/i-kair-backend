using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Salon.API.Infrastructure;
using Salon.API.Models;
using System.Web.Http.OData;
using Salon.API.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Salon.API.Controllers
{
    //[AllowAnonymous]
    [Authorize]
    public class CustomersController : ApiController
    {
        private SalonDataContext db = new SalonDataContext();

        // GET: api/Customers
        [EnableQuery()]
        public IQueryable<CustomerDTO> GetCustomers()
        {
        
            return db.Customers.ProjectTo<CustomerDTO>();
        }

        [Route("api/customers/search/{search}")]
        [HttpGet]
        public IQueryable<Customer> SearchCustomers(string search)
        {
            return db.Customers.Where(c =>
                c.LastName.Contains(search) ||
                c.FirstName.Contains(search) ||
                c.Phone.Contains(search) ||
                c.Email.Contains(search)
            );
        }

        // GET: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // PUT: api/Customers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, CustomerDTO customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerId)
            {
                return BadRequest();
            }
            // 1. Get the appointment itself from the database
            var customerInDb = db.Customers.Find(id);

            // 2. Update appointmentInDb with the VALUES of the appointment object
            customerInDb.Update(customer);

            // 3. Mark appointmentInDb as modified (instead of appointments)
            db.Entry(customerInDb).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Customers
        [ResponseType(typeof(Customer))]
        public IHttpActionResult PostCustomer(CustomerDTO customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dbCustomer = new Customer(customer);
            db.Customers.Add(dbCustomer);
            db.SaveChanges();

            customer.CustomerId = dbCustomer.CustomerId;

            return CreatedAtRoute("DefaultApi", new { id = customer.CustomerId }, Mapper.Map<CustomerDTO>(dbCustomer));
        }

        // DELETE: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok(Mapper.Map<CustomerDTO>(customer));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.CustomerId == id) > 0;
        }
    }
}