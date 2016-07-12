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
using Salon.API.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Salon.API.Controllers
{
    //[AllowAnonymous]
    [Authorize]
    public class ProductsController : ApiController
    {
        private SalonDataContext db = new SalonDataContext();

        // GET: api/Products
        public IQueryable<ProductDTO> GetProducts()
        {
            //return db.Products;
            return db.Products.ProjectTo<ProductDTO>();
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductId)
            {
                return BadRequest();
            }
            // 1. Get the appointment itself from the database
            var productInDb = db.Products.Find(id);

            // 2. Update appointmentInDb with the VALUES of the appointment object
            productInDb.Update(product);

            // 3. Mark appointmentInDb as modified (instead of appointments)
            db.Entry(productInDb).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dbProduct = new Product(product);

            db.Products.Add(dbProduct);
            db.SaveChanges();

            product.ProductId = dbProduct.ProductId;

            return CreatedAtRoute("DefaultApi", new { id = product.ProductId }, Mapper.Map<ProductDTO>(dbProduct));
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(Mapper.Map<ProductDTO>(product));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductId == id) > 0;
        }
    }
}