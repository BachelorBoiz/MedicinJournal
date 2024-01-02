
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
using System.Numerics;
 using MedicinJournal.Security.Models;

 namespace MedicinJournal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalsController : ControllerBase
    {
        private readonly ISymmetricCryptographyService _symmetricKeyService;
        private readonly IAsymmetricCryptographyService _asymmetricKeyService;
        private readonly IJournalService _journalService;
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly IPatientService _patientService;
        private readonly IUserLoginService _userLoginService;

        public JournalsController(IJournalService journalService,
            ISymmetricCryptographyService symmetricKeyService,
            IMapper mapper,
            IEmployeeService employeeService,
            IPatientService patientService,
            IAsymmetricCryptographyService asymmetricKeyService,
            IUserLoginService userLoginService)
        {
            _journalService = journalService;
            _symmetricKeyService = symmetricKeyService;
            _mapper = mapper;
            _employeeService = employeeService;
            _patientService = patientService;
            _asymmetricKeyService = asymmetricKeyService;
            _userLoginService = userLoginService;
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("Patient/Journal/{id}")]
        public async Task<ActionResult<JournalDto>> GetJournalById([FromRoute] int id)
        {
            try
            {
                var journal = await _journalService.GetJournalById(id);

                var patientId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var patient =await _patientService.GetPatientById(patientId);

                var doctor = await _userLoginService.GetUserByDoctorId(patient.Doctor.Id);

                var symmetricKey = await _userLoginService.GetSymmetricKeyByJournalId(journal.Id);

                var signature = await _userLoginService.GetSignatureByJournalId(journal.Id);

                var decryptedJournal = _symmetricKeyService.DecryptText(symmetricKey.Key, journal.Description);
                
                var journalDto = _mapper.Map<JournalDto>(journal);

                if (_asymmetricKeyService.VerifySignature(decryptedJournal, signature.EncryptedHash, doctor.PublicKey))
                {
                    journalDto.Description = decryptedJournal;
                    return Ok(journalDto);
                }

                return BadRequest("Signature could not be verified");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("Doctor/Journal/{id}")]
        public async Task<ActionResult<JournalDto>> DoctorGetJournalById([FromRoute] int id)
        {
            try
            {
                var journal = await _journalService.GetJournalById(id);

                var doctorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var doctor = await _userLoginService.GetUserByDoctorId(doctorId);

                var symmetricKey = await _userLoginService.GetSymmetricKeyByJournalId(journal.Id);

                var signature = await _userLoginService.GetSignatureByJournalId(journal.Id);
                
                var decryptedJournal = _symmetricKeyService.DecryptText(symmetricKey.Key, journal.Description);

                var journalDto = _mapper.Map<JournalDto>(journal);

                if (_asymmetricKeyService.VerifySignature(decryptedJournal, signature.EncryptedHash, doctor.PublicKey))
                {
                    journalDto.Description = decryptedJournal;
                    return Ok(journalDto);
                }

                return BadRequest("Signature could not be verified");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
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
        [HttpPost("CreateJournal/Patient/{patientId}")]
        public async Task<ActionResult<JournalDto>> CreateJournal([FromBody] CreateJournalDto journalDto,[FromRoute] int patientId)
        {
            try
            {
                var doctorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var doctor = await _userLoginService.GetUserByDoctorId(doctorId);

                var symmetricKey = _symmetricKeyService.GenerateKey();
                var iv = _symmetricKeyService.GenerateIV();

                var encryptedText = _symmetricKeyService.EncryptText(symmetricKey, iv, journalDto.Description);

                var journalFromDto = new Journal
                {
                    Title = journalDto.Title,
                    Description = encryptedText
                };

                var signature = _asymmetricKeyService.GenerateSignature(journalDto.Description,
                    _asymmetricKeyService.DeserializeRSAParameters(doctor.PrivateKey));
                
                var newJournal = await _journalService.CreateJournal(journalFromDto, patientId);

                //upload signature
                await _userLoginService.CreateSignature(new Signature
                {
                    EncryptedHash = signature,
                    JournalId = newJournal.Id,
                    TimeStamp = DateTime.Now
                });
                
                //upload Symmetric key
                await _userLoginService.CreateSymmetricKey(new SymmetricKey
                {
                    DoctorId = doctorId,
                    PatientId = patientId,
                    IV = iv,
                    Key = symmetricKey,
                    JournalId = newJournal.Id
                });

                return _mapper.Map<JournalDto>(newJournal);
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
