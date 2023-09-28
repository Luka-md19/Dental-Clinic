using AutoMapper;
using DentalClinic.API.Contract;
using DentalClinic.API.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.API.Repository
{
    public class InvoiceRepository : GenericRepository<Invoice>, INvoiceRepository
    {
        private readonly DentalClinicDbContext _context;

        public InvoiceRepository(DentalClinicDbContext context , IMapper mapper) : base(context, mapper)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.Invoices
                .Where(invoice => invoice.IssueDate >= startDate && invoice.IssueDate <= endDate)
                .ToListAsync();
        }
    }
}
