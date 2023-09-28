using DentalClinic.API.Data;

namespace DentalClinic.API.Contract
{
    public interface INvoiceRepository : IGenericRepository<Invoice>
    {
        Task<IEnumerable<Invoice>> GetInvoicesByDateRange(DateTime startDate, DateTime endDate);
    }
}
