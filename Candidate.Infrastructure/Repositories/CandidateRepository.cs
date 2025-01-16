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

        public async Task<CandidateProfile> GetByEmailAsync(string email)
        {
            return await _context.CandidateProfiles
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Email == email);
        }

        public async Task UpdateAsync(CandidateProfile candidate)
        {
            _context.CandidateProfiles.Update(candidate);
            await _context.SaveChangesAsync();
        }

        public async Task<CandidateProfile> AddAsync(CandidateProfile candidate)
        {
            await _context.CandidateProfiles.AddAsync(candidate);
            await _context.SaveChangesAsync();
            return candidate;
        }
    }
}
