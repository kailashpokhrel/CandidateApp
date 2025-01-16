using Candidate.Domain.Interfaces;
using Candidate.Infrastructure.Persistence;
using Candidate.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using Moq;
using Candidate.Application.Service;

namespace Candidate.Test.Base
{
   
    public class TestSetup
    {
        protected Mock<ICandidateRepository> _mockCandidateRepo;
        protected CandidateService _candidateService;

        public TestSetup()
        {
            // Create mocks for dependencies
            _mockCandidateRepo = new Mock<ICandidateRepository>();

            // Initialize the service under test (injecting the mocked repository)
            _candidateService = new CandidateService(_mockCandidateRepo.Object);
        }
    }
}
