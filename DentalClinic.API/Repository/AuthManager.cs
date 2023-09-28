using AutoMapper;
using DentalClinic.API.Contract;
using DentalClinic.API.Data;
using DentalClinic.API.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DentalClinic.API.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthManager> _logger;
        private ApiUser _user;


        private const string _loginProvider = "DentalClinicAPI";
        private const string _refreshToken = "RefreshToken";
        public AuthManager(IMapper mapper , UserManager<ApiUser> userManager , IConfiguration configuration , ILogger<AuthManager> logger)
        {
            this._mapper = mapper;
            this._userManager = userManager;
            this._configuration = configuration;
            this._logger = logger;
        }
        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            _logger.LogInformation("Login attempt for user: {Email}", loginDto.Email);

            _user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (_user == null)
            {
                _logger.LogWarning("No user found with the email: {Email}", loginDto.Email);
                return null;
            }

            bool isValidUser = await _userManager.CheckPasswordAsync(_user, loginDto.Password);

            if (!isValidUser)
            {
                _logger.LogWarning("Invalid password entered for user: {Email}", loginDto.Email);
                return null;
            }

            _logger.LogInformation("Valid credentials for user: {Email}. Generating token.", loginDto.Email);

            var token = await GenerateToken();
            var refreshToken = await CreateRefreshToken();

            _logger.LogInformation("Token and Refresh Token generated for user: {Email}", loginDto.Email);

            return new AuthResponseDto
            {
                Token = token,
                UserId = _user.Id,
                RefreshToken = refreshToken
            };
        }


        public async Task<IEnumerable<IdentityError>> ValidateUser(ApiUserDto userDto)
        {
            var _user = _mapper.Map<ApiUser>(userDto);
            _user.UserName = userDto.Email;
            var result = await _userManager.CreateAsync(_user,userDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "User");
            }

            return result.Errors;

        }

        private async Task<String> GenerateToken()
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(_user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim("uid", _user.Id),
            }
            .Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IEnumerable<IdentityError>> CreateAdminUser(ApiUserDto userDto)
        {
            var _user = _mapper.Map<ApiUser>(userDto);
            _user.UserName = userDto.Email;
            var result = await _userManager.CreateAsync(_user, userDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "Administrator");
            }

            return result.Errors;
        }

        public async Task<string> CreateRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);
            var result = await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);
            return newRefreshToken;
        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;
            _user = await _userManager.FindByNameAsync(username);

            if (_user == null || _user.Id != request.UserId)
            {
                return null;
            }

            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.RefreshToken);

            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseDto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshToken()
                };
            }

            await _userManager.UpdateSecurityStampAsync(_user);
            return null;
        }
    }
}
