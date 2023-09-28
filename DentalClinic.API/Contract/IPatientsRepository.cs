using DentalClinic.API.Data;
using DentalClinic.API.Models.Patient;

namespace DentalClinic.API.Contract
{
    public interface IPatientsRepository : IGenericRepository<Patient> 
    {
        Task<PatientDto> GetPatientsDetails(int id);
        Task<List<Patient>> GetPatientsWithAppointmentsAfterAsync(DateTime date);
       


    }
}
