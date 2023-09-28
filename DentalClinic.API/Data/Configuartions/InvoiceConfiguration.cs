using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentalClinic.API.Data.Configuartions
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {

            builder.HasData(
                new Invoice { Id = 1, TotalAmount = 100.50, IssueDate = DateTime.Parse("2023-08-10"), PatientId = 1 },
                new Invoice { Id = 2, TotalAmount = 150.75, IssueDate = DateTime.Parse("2023-08-12"), PatientId = 2 }
            );
        }
    }
}
