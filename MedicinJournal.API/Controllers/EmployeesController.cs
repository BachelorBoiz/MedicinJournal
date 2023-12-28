using MedicinJournal.Core.IServices;
using MedicinJournal.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById([FromRoute] int id)
        {
            try
            {
                var employee = await _employeeService.GetById(id);

                return Ok(employee);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
