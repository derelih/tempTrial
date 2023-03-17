using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Display(Name = "Appointment Date")]
        [Required(ErrorMessage = "Please enter an appointment date")]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDate { get; set; }

        [Display(Name = "Patient")]
        [Required(ErrorMessage = "Please select a patient")]
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [Display(Name = "Doctor")]
        [Required(ErrorMessage = "Please select a doctor")]
        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }
    }
}
