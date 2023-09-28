using AutoMapper;
using AutoMapper.QueryableExtensions;
using DentalClinic.API.Contract;
using DentalClinic.API.Data;
using DentalClinic.API.Exceptions;
using DentalClinic.API.Models.Patient;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.API.Repository
{
    public class PatientsRepository : GenericRepository<Patient>, IPatientsRepository
    {
        private readonly DentalClinicDbContext _context;
        private readonly IMapper _mapper;

        public PatientsRepository(DentalClinicDbContext context, IMapper mapper) : base(context, mapper )
        {
            this._context = context;
            this._mapper = mapper;
        }
        public async Task<List<Patient>> GetPatientsWithAppointmentsAfterAsync(DateTime date)
        {
            return await _context.Patients
                                .Include(p => p.Appointments)  // Include related Appointments for each patient
                                .Where(p => p.Appointments.Any(a => a.Date > date)) // Filter patients who have any appointment after the specified date
                                .ToListAsync();
        }
        public async Task<PatientDto> GetPatientsDetails(int id)
        {
            var Patient = await _context.Patients
                       .Include(p => p.Appointments)
                      .Include(p => p.Invoices)
                      .ProjectTo<PatientDto>(_mapper.ConfigurationProvider)
                      .FirstOrDefaultAsync(p => p.Id == id);

            if (Patient == null)
            {
                throw new NotFoundException(nameof(GetPatientsDetails), id);
            }

            return Patient;
        }
    }

        
       
    }

