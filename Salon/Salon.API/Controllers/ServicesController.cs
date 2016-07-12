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
    public class ServicesController : ApiController
    {
        private SalonDataContext db = new SalonDataContext();

        // GET: api/Services
        public IQueryable<ServiceDTO> GetServices()
        {
            //return db.Services;
            return db.Services.ProjectTo<ServiceDTO>();
        }

        // GET: api/Services/5
        [ResponseType(typeof(Service))]
        public IHttpActionResult GetService(int id)
        {
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        // PUT: api/Services/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutService(int id, ServiceDTO service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != service.ServiceId)
            {
                return BadRequest();
            }
            
            var serviceInDb = db.Services.Find(id);
            serviceInDb.Update(service);
            db.Entry(serviceInDb).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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

        // POST: api/Services
        [ResponseType(typeof(Service))]
        public IHttpActionResult PostService(ServiceDTO service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dbService = new Service(service);

            db.Services.Add(dbService);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = service.ServiceId }, Mapper.Map<ServiceDTO>(dbService));
        }

        // DELETE: api/Services/5
        [ResponseType(typeof(Service))]
        public IHttpActionResult DeleteService(int id)
        {
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            db.Services.Remove(service);
            db.SaveChanges();

            return Ok(Mapper.Map<ServiceDTO>(service));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiceExists(int id)
        {
            return db.Services.Count(e => e.ServiceId == id) > 0;
        }
    }
}