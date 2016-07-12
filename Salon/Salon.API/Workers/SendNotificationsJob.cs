using Salon.API.Infrastructure;
using Salon.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twilio;

namespace Salon.API.Workers
{
    public class SendNotificationsJob
    {
        private const string MessageTemplate =
            "Hi {0}. Just a reminder that you have an appointment coming up at {1}.";

        public void Execute()
        {
            using (var db = new SalonDataContext())
            {
                var twilioRestClient = new Domain.Twilio.RestClient();
                var baselineTime = DateTime.Now.AddMinutes(-30);
                var baselineTime2 = DateTime.Now.AddMinutes(30);
                //var upcomingAppointments = db.Appointments.Where(a => DateTime.Now >= a.ScheduleCheckin.AddMinutes(-30) && !a.ReminderSmsSent);
                var upcomingAppointments = db.Appointments.Where(a => a.ScheduleCheckin >= baselineTime && a.ScheduleCheckin <= baselineTime2 && !a.ReminderSmsSent);
                foreach (var appointment in upcomingAppointments.ToList())
                {
                    twilioRestClient.SendSmsMessage(
                        appointment.Customer.Phone,
                        string.Format(MessageTemplate, appointment.Customer.FirstName, appointment.ScheduleCheckin.ToString("t")));

                    appointment.ReminderSmsSent = true;

                    db.Entry(appointment).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                }
            }

        }
    }
}