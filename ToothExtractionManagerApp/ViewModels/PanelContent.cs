using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToothExtractionManager.Data;
using ToothExtractionManager.Data.Models;
using ToothExtractionManagerApp.Utils;

namespace ToothExtractionManagerApp.ViewModels
{
    public class PanelContent 
    {
        public Appointment NewAppointment { get; set; }
        public ObservableCollection<Appointment> CurrentAppointments { get; private set; }
        public ObservableCollection<Appointment> AllPatientsAppointments { get; private set; }

        private readonly AppointmentsService _appointmentService;

        public PanelContent()
        {
            NewAppointment = new Appointment();
            //{
            //    Patient = new PatientInformation()
            //    {
            //        FirstName = "Geralt",
            //        LastName = "Z Rivi",
            //        PhoneNumber = "348394893"
            //    },
            //    Description = "wyrwać wszystkie zęby",
            //    State = AppointmentState.Created,
            //    Date = new DateTime(2019, 05, 31)
            //};
            _appointmentService = new AppointmentsService();
            _appointmentService.Load();
            CurrentAppointments = PrepareCurrentAppointments();
            AllPatientsAppointments = PrepareAllAppointments();
        }

        private ICommand _confirmCommand;
        public ICommand ConfirmCommand
        {
            get
            {
                if (_confirmCommand == null)
                {
                    _confirmCommand = new RelayCommand(
                        param => this.ConfirmAppointment()
                    );
                }
                return _confirmCommand;
            }
        }

        private void ConfirmAppointment()
        {
            
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(
                        param => this.CancelAppointment()
                    );
                }
                return _cancelCommand;
            }
        }

        private void CancelAppointment()
        {

        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        param => this.SaveObject(),
                        param => this.CanSave()
                    );
                }
                return _saveCommand;
            }
        }

        private bool CanSave()
        {
            // Verify command can be executed here
            return true;
        }

        private void SaveObject()
        {
            _appointmentService.AddAppointment(NewAppointment);
            AllPatientsAppointments.Add(NewAppointment);
            _appointmentService.Save();
        }

        private ObservableCollection<Appointment> PrepareCurrentAppointments()
        {
            var currentAppointments = _appointmentService.GetCurrentAppointments();
            var observableAppointments = new ObservableCollection<Appointment>();
            observableAppointments.AddRange(currentAppointments);

            return observableAppointments;
        }

        private ObservableCollection<Appointment> PrepareAllAppointments()
        {
            var allAppointments = _appointmentService.GetAllAppointments();
            var observableAppointments = new ObservableCollection<Appointment>();
            observableAppointments.AddRange(allAppointments);

            return observableAppointments;
        }
    }
}
