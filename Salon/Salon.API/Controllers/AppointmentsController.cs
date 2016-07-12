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
using AutoMapper;
using Salon.API.DTO;
using AutoMapper.QueryableExtensions;

namespace Salon.API.Controllers
{
    //[AllowAnonymous]
    [Authorize]
    public class AppointmentsController : ApiController
    {
        private SalonDataContext db = new SalonDataContext();

        // GET: api/Appointments
        [EnableQuery]
        public IQueryable<AppointmentDTO> GetAppointments()
        {
            return db.Appointments.ProjectTo<AppointmentDTO>();
        }

        // GET: api/Appointments/5
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult GetAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);

            if (appointment == null)
            {
                return NotFound();
            }

            //return Ok(appointment);
            return Ok(Mapper.Map<AppointmentDTO>(appointment));
        }

        // PUT: api/Appointments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAppointment(int id, AppointmentDTO appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointment.AppointmentId)
            {
                return BadRequest();
            }

            // 1. Get the appointment itself from the database
            var appointmentInDb = db.Appointments.Find(id);

            // 2. Update appointmentInDb with the VALUES of the appointment object
            appointmentInDb.Update(appointment);

            // 3. Mark appointmentInDb as modified (instead of appointments)
            db.Entry(appointmentInDb).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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

        // POST: api/Appointments
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult PostAppointment(AppointmentDTO newAppointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the username printed on the incoming token
            string username = User.Identity.Name;

            // Get the actual user from the database (may return null if not found!)
            var user = db.Users.FirstOrDefault(u => u.UserName == username);

            if (user == null) { return Unauthorized(); }

            var stylist = db.Stylists.Find(newAppointment.StylistId);

            int numberOfMinutes = 59;

            if(
                stylist.Appointments.Any(scheduledAppointment => (newAppointment.ScheduleCheckin >= scheduledAppointment.ScheduleCheckin && 
                                                                newAppointment.ScheduleCheckin <= scheduledAppointment.ScheduleCheckin.AddMinutes(numberOfMinutes))
                                                                ||
                                                                (newAppointment.ScheduleCheckin.AddMinutes(numberOfMinutes) >= scheduledAppointment.ScheduleCheckin &&
                                                                newAppointment.ScheduleCheckin.AddMinutes(numberOfMinutes) <= scheduledAppointment.ScheduleCheckin.AddMinutes(numberOfMinutes))
                                                                )
              )
            {
                return BadRequest("The stylist you requested is not available at that time.");
            }
            else
            {
                var dbAppointment = new Appointment(newAppointment);

                dbAppointment.UserId = user.Id;
                dbAppointment.CreatedDate = DateTime.Now;

                db.Appointments.Add(dbAppointment);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = newAppointment.AppointmentId }, Mapper.Map<AppointmentDTO>(dbAppointment));
            }
        }


        //[Route("api/appointments/{appointmentId}/service/{serviceId}")]
        //[HttpPost]
        //public IHttpActionResult AddServiceToAppointment(int appointmentId, int serviceId)
        //{
            //var appointment = db.Appointments.Find(appointmentId);
            //var service = db.Services.Find(serviceId);

            //appointment.Services.Add(service);

            //db.Entry(appointment).State = EntityState.Modified;

            //db.SaveChanges();

            //return Ok();
        //}

        // DELETE: api/Appointments/5
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointment);
            db.SaveChanges();

            //return Ok(appointment);
            return Ok(Mapper.Map<AppointmentDTO>(appointment));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.AppointmentId == id) > 0;
        }
    }
}