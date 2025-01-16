using Candidate.Application.Mapper;
using Candidate.Application.Models;
using Candidate.Domain.Entities;
using Candidate.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate.Application.Service
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;

        public CandidateService(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        public async Task<CandidateDto> GetCandidateByEmailAsync(string email)
        {
            var candidate = await _candidateRepository.GetByEmailAsync(email);
            return candidate == null ? null : CandidateMapper.ToDto(candidate);
        }

        public async Task<CandidateDto> CreateOrUpdateCandidateAsync(CandidateModel candidateModel)
        {
            if (string.IsNullOrWhiteSpace(candidateModel.FirstName))
                throw new ArgumentException("FirstName is required", nameof(candidateModel.FirstName));

            if (string.IsNullOrWhiteSpace(candidateModel.LastName))
                throw new ArgumentException("LastName is required", nameof(candidateModel.LastName));

            if (string.IsNullOrWhiteSpace(candidateModel.Email))
                throw new ArgumentException("Email is required", nameof(candidateModel.Email));

            if (string.IsNullOrWhiteSpace(candidateModel.Comment))
                throw new ArgumentException("Comment  is required", nameof(candidateModel.Comment));

            // Check if the candidate already exists based on the unique email.
            var existingCandidate = await _candidateRepository.GetByEmailAsync(candidateModel.Email);
            if (existingCandidate != null && existingCandidate.CandidateId != candidateModel.CandidateId)
            {
                throw new InvalidOperationException("Candidate with this email already exists.");
            }
            CandidateProfile candidate;
            if (existingCandidate != null)
            {
                // Update existing candidate.
                existingCandidate.FirstName = candidateModel.FirstName;
                existingCandidate.LastName = candidateModel.LastName;
                existingCandidate.PhoneNumber = candidateModel.PhoneNumber;
                existingCandidate.BestTimeToCall = candidateModel.BestTimeToCall;
                existingCandidate.LinkedInProfileUrl = candidateModel.LinkedInProfileUrl;
                existingCandidate.GitHubProfileUrl = candidateModel.GitHubProfileUrl;
                existingCandidate.Comment = candidateModel.Comment;
                existingCandidate.UpdatedAt = DateTime.UtcNow;

                candidate = existingCandidate;
                await _candidateRepository.UpdateAsync(candidate);
            }
            else
            {
                // Create a new candidate if not found.
                candidate = new CandidateProfile
                {
                    FirstName = candidateModel.FirstName,
                    LastName = candidateModel.LastName,
                    PhoneNumber = candidateModel.PhoneNumber,
                    Email = candidateModel.Email,
                    BestTimeToCall = candidateModel.BestTimeToCall,
                    LinkedInProfileUrl = candidateModel.LinkedInProfileUrl,
                    GitHubProfileUrl = candidateModel.GitHubProfileUrl,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Comment = candidateModel.Comment,
                };

                await _candidateRepository.AddAsync(candidate);
            }

            return new CandidateDto
            {
                CandidateId = candidate.CandidateId,
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                PhoneNumber = candidate.PhoneNumber,
                Email = candidate.Email,
                BestTimeToCall = candidate.BestTimeToCall,
                LinkedInProfileUrl = candidate.LinkedInProfileUrl,
                GitHubProfileUrl = candidate.GitHubProfileUrl,
                Comment = candidate.Comment,
            };
        }
    }
}
