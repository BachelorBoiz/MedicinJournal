using MedicinJournal.API.Dtos;
using MedicinJournal.Security.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MedicinJournal.Security.Models;

namespace MedicinJournal.API.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserLoginService _userLoginService;
        public IConfiguration Configuration { get; }

        public JwtService(IConfiguration configuration, IPasswordHasher passwordHasher, IUserLoginService userLoginService)
        {
            Configuration = configuration;
            _passwordHasher = passwordHasher;
            _userLoginService = userLoginService;
        }
        public async Task<JwtToken> GenerateJwt(string userName, string password)
        {
            var user = await _userLoginService.GetUserLogin(userName);

            var userId = 0;

            if (!_passwordHasher.Verify(user.HashedPassword, password))
                return new JwtToken
                {
                    Message = "Patient or Password not correct"
                };

            if (user.Role == UserRole.Employee)
            {
                userId = (int)user.EmployeeId;
            }
            else if (user.Role == UserRole.Patient)
            {
                userId = (int)user.PatientId;
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"],
                Configuration["Jwt:Audience"],
                new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                },
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtToken
            {
                Jwt = new JwtSecurityTokenHandler().WriteToken(token),
                Message = "Ok"
            };
        }
    }
}
