using AutoMapper;
using DentalClinic.API.Data;
using DentalClinic.API.Models.Appointment;
using DentalClinic.API.Models.Dentists;
using DentalClinic.API.Models.Invoice;
using DentalClinic.API.Models.Patient;
using DentalClinic.API.Models.Users;

namespace DentalClinic.API.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Dentist, CreateDentistDto>().ReverseMap();
            CreateMap<Dentist, GetDentistDto>().ReverseMap();
            CreateMap<Dentist, DentistDto>().ReverseMap();
            CreateMap<Dentist, UpdateDentistDto>().ReverseMap();


            CreateMap<Appointment, AppointmentsDto>().ReverseMap();
            CreateMap<Appointment, CreateAppointmentDto>().ReverseMap();


            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<Patient, CreatePatientDto>().ReverseMap();
            CreateMap<Patient, GetPatientDto>().ReverseMap();

            CreateMap<Invoice, InvoiceDto>().ReverseMap();
            CreateMap<Invoice, CreateInvoiceDto>().ReverseMap();


            CreateMap<ApiUserDto, ApiUser>().ReverseMap();


           


        }
    }
}
