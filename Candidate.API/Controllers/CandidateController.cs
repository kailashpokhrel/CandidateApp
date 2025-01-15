using Candidate.Domain.Entities;
using Candidate.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Candidate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateRepository _candidateRepository;

        public CandidateController(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<CandidateProfile>> GetCandidate(string email)
        {
            var candidate = await _candidateRepository.GetByEmailAsync(email);
            if (candidate == null)
            {
                return NotFound(); 
            }
            return Ok(candidate); 
        }

        [HttpPost]
        public async Task<ActionResult<CandidateProfile>> CreateCandidate([FromBody] CandidateProfile candidateProfile)
        {
            var existingCandidate = await _candidateRepository.GetByEmailAsync(candidateProfile.Email);
            if (existingCandidate != null)
            {
                return Conflict("A candidate with the same email already exists."); // HTTP 409 Conflict
            }

            var createdCandidate = await _candidateRepository.AddAsync(candidateProfile);

            return CreatedAtAction(nameof(GetCandidate), new { email = createdCandidate.Email }, createdCandidate);
        }

        [HttpPut("{email}")]
        public async Task<IActionResult> UpdateCandidate(string email, [FromBody] CandidateProfile candidateProfile)
        {
            if (email != candidateProfile.Email)
            {
                return BadRequest("Email in URL does not match email in body."); 
            }

            var existingCandidate = await _candidateRepository.GetByEmailAsync(email);
            if (existingCandidate == null)
            {
                return NotFound(); 
            }

            await _candidateRepository.UpdateAsync(candidateProfile);

            return NoContent(); 
        }

    }
}
