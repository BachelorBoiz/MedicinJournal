
﻿using System.Security.Claims;
using MedicinJournal.Core.IServices;
using MedicinJournal.Core.Models;
﻿using MedicinJournal.Core.IServices;
using MedicinJournal.Security.Interfaces;
using MedicinJournal.Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Reflection.Metadata;
 using AutoMapper;
 using MedicinJournal.API.Dtos;

 namespace MedicinJournal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalsController : ControllerBase
    {
        private readonly ISymmetricCryptographyService _symmetricKeyService;
        private readonly IJournalService _journalService;
        private readonly IMapper _mapper;

        public JournalsController(IJournalService journalService, ISymmetricCryptographyService symmetricKeyService, IMapper mapper)
        {
            _journalService = journalService;
            _symmetricKeyService = symmetricKeyService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("journal/{id}")]
        public async Task<ActionResult> GetJournalById([FromRoute] int id)
        {
            var journal = await _journalService.GetJournalById(id);

            return Ok(journal);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("patientJournals")]
        public async Task<ActionResult<IEnumerable<JournalDto>>> GetJournalsForPatient()
        {
            try
            {
                var patientId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var journals = await _journalService.GetJournalsForUser(patientId);

                var journalsDto = _mapper.Map<IEnumerable<JournalDto>>(journals);

                return Ok(journalsDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Employee")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteJournalById([FromRoute] int id)
        {
            await _journalService.DeleteJournal(id);
            return Ok($"Journal entry {id} deleted successfully");
        }
    }
}
