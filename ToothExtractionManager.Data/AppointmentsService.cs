using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.WebSockets;
using ToothExtractionManager.Data.Models;

namespace ToothExtractionManager.Data
{
    public class AppointmentsService
    {
        private List<Appointment> _appointments = new List<Appointment>();

        public void AddAppointment(Appointment appointment)
        {
            _appointments.Add(appointment);
        }

        public List<Appointment> GetCurrentAppointments()
        {
            // show upcoming or in-progress (hence 15 minutes delay) confirmed appointments
            return _appointments
                .Where(x => x.State == AppointmentState.Confirmed && x.Date >= DateTime.Now.AddMinutes(-15.0))
                .ToList();
        }

        public List<Appointment> GetAllAppointments()
        {
            // show all appointments - for receptionist
            return _appointments.ToList();
        }

        private const string StorageFileName = @".\tooth.json";

        public void Save()
        {
            File.WriteAllText(StorageFileName, JsonConvert.SerializeObject(_appointments, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat
                }));
        }

        public void Load()
        {
            try
            {
                _appointments = JsonConvert.DeserializeObject<List<Appointment>>(File.ReadAllText(StorageFileName),
                    new JsonSerializerSettings()
                    {
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });
            }
            catch (Exception e)
            {
                _appointments = new List<Appointment>();
            }
        }
    }
}
