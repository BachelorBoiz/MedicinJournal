using System.Security.Claims;
using MedicinJournal.Core.IServices;
using MedicinJournal.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicinJournal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalsController : ControllerBase
    {
        private readonly IJournalService _journalService;
        public JournalsController(IJournalService journalService) 
        {
            _journalService = journalService;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetJournalById([FromRoute] int id)
        {
            var journal = await _journalService.GetJournalById(id);

            return Ok(journal);
        }

        [Authorize(Roles = "Doctor, Patient")]
        [HttpGet("userJournals")]
        public async Task<ActionResult<IEnumerable<Journal>>> GetJournalsForUser()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var journals = await _journalService.GetJournalsForUser(userId);

                return Ok(journals);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
