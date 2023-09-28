using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentalClinic.API.Data.Configuartions
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasData(
                new Appointment { Id = 1, Date = DateTime.Parse("2023-08-15"), DentistId = 1, PatientId = 1 },
                new Appointment { Id = 2, Date = DateTime.Parse("2023-08-16"), DentistId = 2, PatientId = 2 }
            );
        }
    }
}
