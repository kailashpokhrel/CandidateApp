using Candidate.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate.Application.Service
{
    public interface ICandidateService
    {
        Task<CandidateDto> GetCandidateByEmailAsync(string email);
        Task<CandidateDto> CreateOrUpdateCandidateAsync(CandidateModel candidateModel);

    }
}
