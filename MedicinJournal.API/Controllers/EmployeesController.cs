using MedicinJournal.Core.IServices;
using MedicinJournal.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MedicinJournal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("LoggedInEmployeeInfo")]
        public async Task<ActionResult<Employee>> GetLoggedInEmployeeInfo()
        {
            try
            {
                var employeeId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var employee = await _employeeService.GetById(employeeId);

                return Ok(employee);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
