using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ToothExtractionManager.Data;
using ToothExtractionManager.Data.Models;
using ToothExtractionManagerApp.Utils;

namespace ToothExtractionManagerApp.ViewModels
{
    public class CommandedAppointment : Appointment
    {
        private readonly Appointment _referencedAppointment;

        public Action RefreshAppointments { get; set; }

        public CommandedAppointment(Appointment originalAppointment) : base(originalAppointment)
        {
            _referencedAppointment = originalAppointment;
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
            this.State = AppointmentState.Confirmed;
            _referencedAppointment.State = AppointmentState.Confirmed;
            RefreshAppointments();
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
            this.State = AppointmentState.Cancelled;
            _referencedAppointment.State = AppointmentState.Cancelled;
            RefreshAppointments();
        }
    }

    public class PanelContent 
    {
        public Appointment NewAppointment { get; set; }
        public ObservableCollection<Appointment> CurrentAppointments { get; private set; }
        public ObservableCollection<CommandedAppointment> AllPatientsAppointments { get; private set; }

        public AppointmentsService AppointmentService;

        public PanelContent()
        {
            NewAppointment = new Appointment();
            AppointmentService = new AppointmentsService();
            AppointmentService.Load();
            CurrentAppointments = new ObservableCollection<Appointment>();
            FillCurrentAppointments();
            AllPatientsAppointments = new ObservableCollection<CommandedAppointment>();
            FillAllAppointments();
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

        private void RefreshAppointments()
        {
            AppointmentService.Save();
            FillAllAppointments();
            FillCurrentAppointments();
        }

        private void SaveObject()
        {
            AppointmentService.AddAppointment(NewAppointment);
            AllPatientsAppointments.Add(new CommandedAppointment(NewAppointment)
            {
                RefreshAppointments = this.RefreshAppointments
            });
            AppointmentService.Save();
        }

        private void FillCurrentAppointments()
        {
            var currentAppointments = AppointmentService.GetCurrentAppointments();
            CurrentAppointments.Clear();
            CurrentAppointments.AddRange(currentAppointments);
        }

        private void FillAllAppointments()
        {
            var allAppointments = AppointmentService.GetAllAppointments().Select(x => new CommandedAppointment(x)
            {
                RefreshAppointments = this.RefreshAppointments
            }).ToList();
            AllPatientsAppointments.Clear();
            AllPatientsAppointments.AddRange(allAppointments);
        }
    }
}
