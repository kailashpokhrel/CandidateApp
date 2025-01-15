using Candidate.Domain.Entities;
using Candidate.Domain.Interfaces;
using Candidate.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Candidate.Infrastructure.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationsDbContext _context;

        public CandidateRepository(ApplicationsDbContext context)
        {
            _context = context;
        }
        public async Task<CandidateProfile> AddAsync(CandidateProfile candidate)
        {
            await _context.CandidateProfiles.AddAsync(candidate); 
            await _context.SaveChangesAsync();
            return candidate;

        }

        public async Task<CandidateProfile> GetByEmailAsync(string email)
        {
             return await _context.CandidateProfiles.FirstOrDefaultAsync(c => c.Email == email);

        }
     
        public async Task UpdateAsync(CandidateProfile candidate)
        {
            var existingCandidate = await _context.CandidateProfiles
                .FirstOrDefaultAsync(c => c.Email == candidate.Email);

            if (existingCandidate == null)
            {
                throw new KeyNotFoundException("Candidate not found.");
            }

            existingCandidate.FirstName = candidate.FirstName;
            existingCandidate.LastName = candidate.LastName;
            existingCandidate.PhoneNumber = candidate.PhoneNumber;
            existingCandidate.BestTimeToCall = candidate.BestTimeToCall;
            existingCandidate.LinkedInProfileUrl = candidate.LinkedInProfileUrl;
            existingCandidate.GitHubProfileUrl = candidate.GitHubProfileUrl;
            existingCandidate.Comment = candidate.Comment;

            await _context.SaveChangesAsync();
        }
    }
}
