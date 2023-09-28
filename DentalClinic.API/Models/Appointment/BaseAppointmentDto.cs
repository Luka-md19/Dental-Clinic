using System.ComponentModel.DataAnnotations;

namespace DentalClinic.API.Models.Appointment

{
    public abstract class BaseAppointmentDto
    {

        public DateTime Date { get; set; }
        [Required]
        
        public int PatientId { get; set; }
        [Required]
       
        public int DentistId { get; set; }
    }
}
