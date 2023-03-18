using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyApp.Models;

namespace MyApp.ViewModels
{
    public class AppointmentFormViewModel
    {
        public int? Id { get; set; }

        [Display(Name = "Appointment Date")]
        [Required(ErrorMessage = "Please enter an appointment date")]
        public string AppointmentDate { get; set; }

        [Display(Name = "Patient")]
        [Required(ErrorMessage = "Please select a patient")]
        public int PatientId { get; set; }

        [Display(Name = "Doctor")]
        [Required(ErrorMessage = "Please select a doctor")]
        public int DoctorId { get; set; }

        public IEnumerable<Doctor> Doctors { get; set; }

        public IEnumerable<Patient> Patients { get; set; }

        public string Title => Id != 0 ? "Edit Appointment" : "New Appointment";

        public AppointmentFormViewModel()
        {
            Id = 0;
            AppointmentDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
        }

        public AppointmentFormViewModel(Appointment appointment)
        {
            Id = appointment.Id;
            AppointmentDate = appointment.AppointmentDate.ToString("yyyy-MM-ddTHH:mm");
            PatientId = appointment.PatientId;
            DoctorId = appointment.DoctorId;
        }
    }
}
