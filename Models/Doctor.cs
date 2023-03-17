using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter a first name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter a last name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Specialty")]
        [Required(ErrorMessage = "Please enter a specialty")]
        [StringLength(50)]
        public string Specialty { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
