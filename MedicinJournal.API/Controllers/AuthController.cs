using MedicinJournal.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Security.Claims;
using MedicinJournal.API.Jwt;
using MedicinJournal.Core.IServices;
using MedicinJournal.Core.Models;
using MedicinJournal.Security.Interfaces;
using MedicinJournal.Security.Models;

namespace MedicinJournal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUserLoginService _userLoginService;
        private readonly IPatientService _patientService;

        public AuthController(IJwtService jwtService, IUserLoginService userLoginService, IPatientService patientService)
        {
            _jwtService = jwtService;
            _userLoginService = userLoginService;
            _patientService = patientService;
        }

        [Authorize(Roles = "Employee")]
        [HttpPost("CreatePatient")]
        public async Task<ActionResult<JwtToken>> CreatePatient([FromBody] CreateUserDto dto)
        {
            var user = await _userLoginService.GetUserLogin(dto.UserName);

            if (user is not null) return BadRequest("Patient with given username already exists");

            var newUser = await _patientService.CreatePatient(new Patient
            {
                BirthDate = dto.BirthDate,
                Gender = dto.Gender,
                Height = dto.Height,
                Name = dto.Name,
                Weight = dto.Weight
            });

            var newUserLogin = await _userLoginService.CreateUserLogin(newUser.Id, dto.UserName, dto.PlainTextPassword);

            var token = await _jwtService.GenerateJwt(newUserLogin.UserName, dto.PlainTextPassword);

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<JwtToken>> Login([FromBody] LoginDto dto)
        {
            try
            {
                var token = await _jwtService.GenerateJwt(dto.UserName, dto.Password);
                return new JwtToken
                {
                    Jwt = token.Jwt,
                    Message = token.Message
                };
            }
            catch (AuthenticationException ae)
            {
                return Unauthorized(ae.Message);
            }
        }

        [Authorize]
        [HttpGet("userRole")]
        public async Task<ActionResult<GetUserRole>> GetUserRole()
        {
            try
            {
                var userName = User.FindFirstValue(ClaimTypes.Name);

                var role =  await _userLoginService.GetUserRole(userName);

                var getRole = new GetUserRole
                {
                    Name = role.ToString()
                };

                return Ok(getRole);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
