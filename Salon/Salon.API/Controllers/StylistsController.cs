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
using System.Web.Http.OData;

namespace Salon.API.Controllers
{
    //[AllowAnonymous]
    [Authorize]
    public class StylistsController : ApiController
    {
        private SalonDataContext db = new SalonDataContext();

        // GET: api/Stylists
        [EnableQuery]
        public IQueryable<StylistDTO> GetStylists()
        {
            //return db.Stylists;
            return db.Stylists.ProjectTo<StylistDTO>();
        }

        // GET: api/Stylists/5
        [ResponseType(typeof(Stylist))]
        public IHttpActionResult GetStylist(int id)
        {
            Stylist stylist = db.Stylists.Find(id);
            if (stylist == null)
            {
                return NotFound();
            }

            return Ok(stylist);
        }

        // PUT: api/Stylists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStylist(int id, StylistDTO stylist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stylist.StylistId)
            {
                return BadRequest();
            }
            var stylistInDb = db.Stylists.Find(id);
            stylistInDb.Update(stylist);
            db.Entry(stylistInDb).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StylistExists(id))
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

        // POST: api/Stylists
        [ResponseType(typeof(Stylist))]
        public IHttpActionResult PostStylist(StylistDTO stylist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dbStylist = new Stylist(stylist);
            db.Stylists.Add(dbStylist);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = stylist.StylistId }, Mapper.Map<StylistDTO>(dbStylist));
        }

        // DELETE: api/Stylists/5
        [ResponseType(typeof(Stylist))]
        public IHttpActionResult DeleteStylist(int id)
        {
            Stylist stylist = db.Stylists.Find(id);
            if (stylist == null)
            {
                return NotFound();
            }

            db.Stylists.Remove(stylist);
            db.SaveChanges();

            return Ok(Mapper.Map<StylistDTO>(stylist));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StylistExists(int id)
        {
            return db.Stylists.Count(e => e.StylistId == id) > 0;
        }
    }
}