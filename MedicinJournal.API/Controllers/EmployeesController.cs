using MedicinJournal.Core.IServices;
using MedicinJournal.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;
using MedicinJournal.API.Dtos;

namespace MedicinJournal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("LoggedInEmployeeInfo")]
        public async Task<ActionResult<EmployeeDto>> GetLoggedInEmployeeInfo()
        {
            try
            {
                var employeeId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var employee = await _employeeService.GetById(employeeId);

                var employeeDto = _mapper.Map<EmployeeDto>(employee);

                return Ok(employeeDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
} 
