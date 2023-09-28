using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentalClinic.API.Data.Configuartions
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
          builder.HasData(
                new Patient { Id = 1, Name = "John Doe", DateOfBirth = DateTime.Parse("1990-01-01"), PhoneNumber = "123-456-7890" },
                new Patient { Id = 2, Name = "Jane Smith", DateOfBirth = DateTime.Parse("1985-05-15"), PhoneNumber = "987-654-3210" }
            );
        }
    }
}
