using AutoMapper;
using DentalClinic.API.Contract;
using DentalClinic.API.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.API.Repository
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {

        public AppointmentRepository(DentalClinicDbContext context , IMapper mapper) : base(context, mapper)
        {
        }
       
    }
}
