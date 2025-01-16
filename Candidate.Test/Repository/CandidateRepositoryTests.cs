using Candidate.Domain.Entities;
using Candidate.Infrastructure.Persistence;
using Candidate.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using FluentAssertions;
using Xunit;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Candidate.Domain.Interfaces;

namespace Candidate.Test.Repository
{
    public class CandidateRepositoryTests
    {
        private readonly ApplicationsDbContext _context;
        private readonly CandidateRepository _repository;

        public CandidateRepositoryTests()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationsDbContext(options); // Use InMemoryDbContext
            _repository = new CandidateRepository(_context);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnCandidate_WhenExists()
        {
            // Arrange
            var email = "test@test.com";
            var candidate = new CandidateProfile
            {
                FirstName = "test candidate",
                LastName = "test",
                Email = email,
                PhoneNumber = "1234567890",
                BestTimeToCall = "9AM-5PM",
                LinkedInProfileUrl = "https://linkedin.com/test",
                GitHubProfileUrl = "https://github.com/test",
                Comment = "Test candidate"
            };
            await _context.CandidateProfiles.AddAsync(candidate);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByEmailAsync(email);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(candidate.Email);
            result.FirstName.Should().Be(candidate.FirstName);
            result.LastName.Should().Be(candidate.LastName);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnNull_WhenCandidateDoesNotExist()
        {
            // Arrange
            var email = "nonexistent@test.com";

            // Act
            var result = await _repository.GetByEmailAsync(email);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ShouldAddCandidate_WhenValid()
        {
            // Arrange
            var candidate = new CandidateProfile
            {
                FirstName = "test",
                LastName = "tester",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                BestTimeToCall = "9AM-5PM",
                LinkedInProfileUrl = "https://linkedin.com/kailash",
                GitHubProfileUrl = "https://github.com/kailash",
                Comment = "Experienced software engineer."
            };

            // Act
            var result = await _repository.AddAsync(candidate);
            await _context.SaveChangesAsync(); 

            // Assert
            result.Should().NotBeNull();
            var savedCandidate = await _context.CandidateProfiles.FindAsync(candidate.CandidateId);
            savedCandidate.Should().NotBeNull();
            savedCandidate.FirstName.Should().Be(candidate.FirstName);
            savedCandidate.LastName.Should().Be(candidate.LastName);
            savedCandidate.Email.Should().Be(candidate.Email);
            savedCandidate.PhoneNumber.Should().Be(candidate.PhoneNumber);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateCandidate_WhenValid()
        {
            // Arrange
            var candidate = new CandidateProfile
            {
                Email = "test@test.com",
                FirstName = "test",
                LastName = "tester",
                PhoneNumber = "123456789",
                BestTimeToCall = "9AM-5PM",
                LinkedInProfileUrl = "https://linkedin.com/test",
                GitHubProfileUrl = "https://github.com/test",
                Comment = "Experienced software engineer."
            };
            await _context.CandidateProfiles.AddAsync(candidate);
            // Save the candidate to the in-memory DB
            await _context.SaveChangesAsync(); 

            // Modify the candidate
            candidate.FirstName = "test1";
            candidate.LastName = "tester1";

            // Act
            await _repository.UpdateAsync(candidate);
            await _context.SaveChangesAsync();

            // Assert
            var updatedCandidate = await _context.CandidateProfiles.FindAsync(candidate.CandidateId);
            updatedCandidate.Should().NotBeNull();
            updatedCandidate.FirstName.Should().Be("test1");
            updatedCandidate.LastName.Should().Be("tester1");
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenDuplicateEmailIsAdded()
        {
            // Arrange
            var candidate1 = new CandidateProfile
            {
                FirstName = "test",
                LastName = "tester",
                Email = "test@example.com",
                PhoneNumber = "12345678911",
                BestTimeToCall = "9AM-5PM",
                LinkedInProfileUrl = "https://linkedin.com/kailash",
                GitHubProfileUrl = "https://github.com/kailash",
                Comment = "Software Engineer"
            };
            await _context.CandidateProfiles.AddAsync(candidate1);
            await _context.SaveChangesAsync();

            var candidate2 = new CandidateProfile
            {
                FirstName = "test",
                LastName = "tester",
                Email = "test@example.com", // Duplicate email
                PhoneNumber = "98765432111",
                BestTimeToCall = "9AM-5PM",
                LinkedInProfileUrl = "https://linkedin.com/kailash",
                GitHubProfileUrl = "https://github.com/kailash",
                Comment = "Software Engineer"
            };

            // Act & Assert
            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                // This should throw due to duplicate email
                await _repository.AddAsync(candidate2);  
            });

            Assert.Equal("Candidate with this email already exists.", exception.Result.Message);
        }
    }
}
