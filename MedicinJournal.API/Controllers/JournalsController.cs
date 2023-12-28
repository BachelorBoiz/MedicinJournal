
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

namespace MedicinJournal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalsController : ControllerBase
    {
        private readonly ISymmetricCryptographyService _symmetricKeyService;
        private readonly IJournalService _journalService;

        public JournalsController(IJournalService journalService, ISymmetricCryptographyService symmetricKeyService)
        {
            _journalService = journalService;
            _symmetricKeyService = symmetricKeyService;
        }

        //[Authorize]
        [HttpGet("journal/{id}")]
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
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Doctor")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteJournalById([FromRoute] int id)
        {
            await _journalService.DeleteJournal(id);
            return Ok($"Jornal entry {id} deleted successfully");

        }

        /// <summary>
        /// Testing if the symmetric key encryption works, Console is the docker log
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>

        [HttpGet("symmetric-encryption/{text}")]
        public ActionResult<string> TestSymmetricEncryption([FromRoute]string text)
        {
            byte[] symmetricKey = _symmetricKeyService.GenerateKey();
            byte[] iv = _symmetricKeyService.GenerateIV();

            //encrypt
            var encryptedText = _symmetricKeyService.EncryptText(symmetricKey, iv, text);

            //decrypt
            var deryptedText = _symmetricKeyService.DecryptText(symmetricKey, encryptedText);

            //Test
            Console.WriteLine("Symmetric Key Test---------------------------------");
            Console.WriteLine($"Symmetric key = {symmetricKey}");
            Console.WriteLine($"IV = {iv}");
            Console.WriteLine($"Text before encryption = {text}");
            Console.WriteLine($"Encrypted text = {encryptedText}");
            Console.WriteLine($"Decrypted Text = {deryptedText}");
            Console.WriteLine("Symmetric Key Test---------------------------------");
            return Ok(deryptedText);
        }
    }
}
