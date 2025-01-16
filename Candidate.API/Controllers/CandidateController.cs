using Candidate.Application.Models;
using Candidate.Application.Service;
using Candidate.Domain.Entities;
using Candidate.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Candidate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateService _candidateService;
        private readonly ILogger<CandidateController> _logger;


        public CandidateController(ICandidateService candidateService, ILogger<CandidateController> logger)
        {
            _candidateService = candidateService;
            _logger = logger ;
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<CandidateDto>> GetCandidate(string email)
        {
            try
            {
                var candidate = await _candidateService.GetCandidateByEmailAsync(email);
                if (candidate == null)
                {
                    return NotFound();
                }
                return Ok(candidate);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error retriving candidate");
                return StatusCode(500, "Internal Server Error");
            }
           
        }

        
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdateCandidateAsync([FromBody] CandidateModel candidateModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {

                var candidate = await _candidateService.CreateOrUpdateCandidateAsync(candidateModel);
                
                if (candidate == null)
                {
                    return NotFound(new { message = "Candidate not found" });  // Return 404 if candidate is null
                }
                
                return Ok(candidate);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
            {
                return Conflict(new { message = ex.Message }); 
            }
            catch (System.Exception ex)
            {
                //_logger.LogError(ex, "Error creating or updating candidate.");
                return StatusCode(500, "Internal Server Error");
            }
        }


    }
}
