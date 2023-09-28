using System.ComponentModel.DataAnnotations;

namespace DentalClinic.API.Models.Patient
{
    public abstract class BasePatientDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name should be between 2 and 100 characters.")]
        [RegularExpression("^[a-zA-Z-' ]*$", ErrorMessage = "Name can only contain letters and spaces.")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Date)] 
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, ErrorMessage = "Phone number can't be longer than 15 digits.")]
        public string PhoneNumber { get; set; }
    }
}
