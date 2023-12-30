using System.Security.Claims;
using AutoMapper;
using MedicinJournal.API.Dtos;
using MedicinJournal.Core.IServices;
using MedicinJournal.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicinJournal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;

        public PatientsController(IPatientService patientService, IMapper mapper)
        {
            _patientService = patientService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("LoggedInPatientInfo")]
        public async Task<ActionResult<PatientDto>> GetLoggedInPatient()
        {
            try
            {
                var patientId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var patient = await _patientService.GetPatientById(patientId);
                
                var patientDto = _mapper.Map<PatientDto>(patient);

                return Ok(patientDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetPatientById([FromRoute] int id)
        {
            try
            {
                var patient = await _patientService.GetPatientById(id);

                var patientDto = _mapper.Map<PatientDto>(patient);

                return Ok(patientDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
