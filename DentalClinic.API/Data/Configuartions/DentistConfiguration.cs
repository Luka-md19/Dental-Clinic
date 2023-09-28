using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.API.Data.Configuartions
{
    public class DentistConfiguration : IEntityTypeConfiguration<Dentist>
    {
        public void Configure(EntityTypeBuilder<Dentist> builder)
        {
            builder.HasData(
                new Dentist { Id = 1, Name = "Dr. Smith" },
                new Dentist { Id = 2, Name = "Dr. Johnson" },
                new Dentist { Id = 3, Name = "Dr. Flafel" }
            );
        }
    }
}


