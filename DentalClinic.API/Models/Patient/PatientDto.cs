using DentalClinic.API.Models.Appointment;
using DentalClinic.API.Models.Invoice;

namespace DentalClinic.API.Models.Patient
{
    public class PatientDto : BasePatientDto
    {
        public int Id { get; set; }
        public  IList<AppointmentsDto> Appointments { get; set; }
        public  IList<InvoiceDto> Invoices { get; set; }


        public PatientDto()
        {
            Appointments = new List<AppointmentsDto>();
            Invoices = new List<InvoiceDto>();
        }

    }
}
