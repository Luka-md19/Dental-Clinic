using Microsoft.Build.Framework;

namespace DentalClinic.API.Models.Dentists
{
    public  abstract class BaseDentistDto
    {
        [Required]
        public string Name { get; set; }
        public string Specialization { get; set; }

    }
}
