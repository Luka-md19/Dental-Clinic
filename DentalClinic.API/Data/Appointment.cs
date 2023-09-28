using System.ComponentModel.DataAnnotations.Schema;

namespace DentalClinic.API.Data
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey(nameof(PatientId))]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey(nameof(DentistId))]
        public int DentistId { get; set; }
        public Dentist Dentist { get; set; }
        
    }

   


}
 