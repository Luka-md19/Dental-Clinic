using DentalClinic.API.Data;
using DentalClinic.API.Models.Dentists;

namespace DentalClinic.API.Contract
{
    public interface IDentistRepository : IGenericRepository<Dentist>
    {
        Task<DentistDto> GetDetails(int id);
        Task<List<Dentist>> FindBySpecializationAsync(string specialization);
        Task<bool> HasAppointmentsAsync(int dentistId);
    }
}
