using Microsoft.AspNetCore.Identity;

namespace DentalClinic.API.Data
{
    public class ApiUser : IdentityUser
    {
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
