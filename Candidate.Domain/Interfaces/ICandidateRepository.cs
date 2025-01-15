using Candidate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate.Domain.Interfaces
{
    public interface ICandidateRepository
    {
        Task<CandidateProfile> GetByEmailAsync(string email);
        Task<CandidateProfile> AddAsync(CandidateProfile candidate);
        Task UpdateAsync(CandidateProfile candidate);


    }
}
