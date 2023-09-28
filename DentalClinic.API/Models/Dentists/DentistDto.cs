using DentalClinic.API.Models.Appointment;

namespace DentalClinic.API.Models.Dentists
{
    public class DentistDto : BaseDentistDto
    {

        public int Id { get; set; }
        public IList<AppointmentsDto> Appointments { get; set; }
    }
}
