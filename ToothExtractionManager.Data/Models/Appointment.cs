using System;

namespace ToothExtractionManager.Data.Models
{
    public enum AppointmentState
    {
        Created,
        Confirmed,
        Cancelled
    }

    public class Appointment
    {
        public Appointment()
        {
            Patient = new PatientInformation();
            Date = DateTime.Now;
            State = AppointmentState.Created;
        }

        public Appointment(Appointment copy)
        {
            Patient = copy.Patient;
            Date = copy.Date;
            Description = copy.Description;
            State = copy.State;
        }

        public PatientInformation Patient { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public AppointmentState State { get; set; }
    }
}
