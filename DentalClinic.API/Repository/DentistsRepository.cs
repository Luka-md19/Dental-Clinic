using AutoMapper;
using AutoMapper.QueryableExtensions;
using DentalClinic.API.Contract;
using DentalClinic.API.Data;
using DentalClinic.API.Exceptions;
using DentalClinic.API.Models.Dentists;
using Microsoft.EntityFrameworkCore;
namespace DentalClinic.API.Repository
{
    public class DentistsRepository : GenericRepository<Dentist>, IDentistRepository
    {
        private readonly DentalClinicDbContext _context;
        private readonly IMapper _mapper;

        public DentistsRepository(DentalClinicDbContext context, IMapper mapper) : base(context , mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
      
        public async Task<DentistDto> GetDetails(int id)
        {
            var Dentist = await _context.Dentists.Include(q => q.Appointments)
                .ProjectTo<DentistDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (Dentist == null)
            {
                throw new NotFoundException(nameof(GetDetails), id);
            }

            return Dentist;
        }
        public async Task<List<Dentist>> FindBySpecializationAsync(string specialization)
        {
            
            return await _context.Dentists
                .Where(d => d.Specialization == specialization)
                .ToListAsync();
        }

        public async Task<bool> HasAppointmentsAsync(int dentistId)
        {
            return await _context.Appointments
                .AnyAsync(a => a.DentistId == dentistId);
        }
    }
}