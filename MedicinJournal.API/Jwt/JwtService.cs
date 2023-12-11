using MedicinJournal.API.Dtos;
using MedicinJournal.Security.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

            if (!_passwordHasher.Verify(user.HashedPassword, password))
                return new JwtToken
                {
                    Message = "User or Password not correct"
                };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"],
                Configuration["Jwt:Audience"],
                new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
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
