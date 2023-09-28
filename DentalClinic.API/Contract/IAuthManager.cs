using DentalClinic.API.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace DentalClinic.API.Contract
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> ValidateUser(ApiUserDto userDto);
        Task<AuthResponseDto> Login(LoginDto login);
        Task<IEnumerable<IdentityError>> CreateAdminUser(ApiUserDto userDto);

        Task<string> CreateRefreshToken();
        Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);


    }
}
