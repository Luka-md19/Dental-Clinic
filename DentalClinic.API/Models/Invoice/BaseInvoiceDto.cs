using DentalClinic.API.Models.Patient;
using System.ComponentModel.DataAnnotations;

namespace DentalClinic.API.Models.Invoice
{
    public abstract class BaseInvoiceDto
    {
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount should be a positive value.")]
        public double TotalAmount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1/1/1900", "12/31/2099", ErrorMessage = "Issue date should be between 1900 and 2099.")]
        public DateTime IssueDate { get; set; }
        [Required]
        public int PatientId { get; set; }
        
       
    }
}
