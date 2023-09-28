using System.ComponentModel.DataAnnotations.Schema;

namespace DentalClinic.API.Data
{
    public class Invoice
    {
        public int Id { get; set; }

        [ForeignKey(nameof(PatientId))]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public double TotalAmount { get; set; }
        public DateTime IssueDate { get; set; }

      
    }

   


}
