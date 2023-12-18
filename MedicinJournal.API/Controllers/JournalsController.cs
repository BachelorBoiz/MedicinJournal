using MedicinJournal.Core.IServices;
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
    }
}
