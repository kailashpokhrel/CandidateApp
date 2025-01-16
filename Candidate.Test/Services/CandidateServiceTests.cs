using Candidate.Application.Models;
using Candidate.Application.Service;
using Candidate.Domain.Interfaces;
using Moq;
using System.Threading.Tasks;
using Candidate.Domain.Entities;
using Xunit;

namespace Candidate.Test.Services
{
    public class CandidateServiceTests 
    {
        private readonly Mock<ICandidateRepository> _candidateRepoMock;
        private readonly CandidateService _service;

        public CandidateServiceTests()
        {
            _candidateRepoMock = new Mock<ICandidateRepository>();
            _service = new CandidateService(_candidateRepoMock.Object);
        }

        [Fact]
        public async Task CreateOrUpdateCandidateAsync_ShouldCreateNewCandidate_WhenNotExist()
        {
            var candidateModel = new CandidateModel
            {
                FirstName = "test",
                LastName = "tester",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Comment = "Test Comment"
            };

            _candidateRepoMock.Setup(repo => repo.GetByEmailAsync(candidateModel.Email)).ReturnsAsync((CandidateProfile)null);
            _candidateRepoMock.Setup(repo => repo.AddAsync(It.IsAny<CandidateProfile>())).ReturnsAsync(new CandidateProfile
            {
                FirstName = candidateModel.FirstName,
                LastName = candidateModel.LastName,
                Email = candidateModel.Email,
                PhoneNumber = candidateModel.PhoneNumber,
                Comment = candidateModel.Comment
            });

            var result = await _service.CreateOrUpdateCandidateAsync(candidateModel);

            Assert.NotNull(result);
            Assert.Equal(candidateModel.Email, result.Email);
            Assert.Equal(candidateModel.FirstName, result.FirstName);
            Assert.Equal(candidateModel.LastName, result.LastName);
            Assert.Equal(candidateModel.Comment, result.Comment);
            _candidateRepoMock.Verify(repo => repo.AddAsync(It.IsAny<CandidateProfile>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrUpdateCandidateAsync_ShouldUpdateCandidate_WhenExist()
        {
            var candidateModel = new CandidateModel
            {
                CandidateId = 1,
                FirstName = "test",
                LastName = "tester",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Comment = "Updated Comment"
            };

            var existingCandidate = new CandidateProfile
            {
                CandidateId = 1,
                FirstName = "test",
                LastName = "tester",
                Email = "test@example.com",
                PhoneNumber = "9876543210",
                Comment = "Old Comment"
            };

            _candidateRepoMock.Setup(repo => repo.GetByEmailAsync(candidateModel.Email)).ReturnsAsync(existingCandidate);

            var result = await _service.CreateOrUpdateCandidateAsync(candidateModel);

            Assert.NotNull(result);
            Assert.Equal(candidateModel.Comment, result.Comment);
            Assert.Equal(candidateModel.FirstName, result.FirstName);
            Assert.Equal(candidateModel.LastName, result.LastName);
            _candidateRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<CandidateProfile>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrUpdateCandidateAsync_ShouldThrowException_WhenRepositoryFails()
        {
            var candidateModel = new CandidateModel
            {
                FirstName = "test",
                LastName = "tester",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Comment = "Test Comment"
            };

            _candidateRepoMock.Setup(repo => repo.GetByEmailAsync(candidateModel.Email)).ReturnsAsync((CandidateProfile)null);
            _candidateRepoMock.Setup(repo => repo.AddAsync(It.IsAny<CandidateProfile>())).ThrowsAsync(new System.Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(async () => await _service.CreateOrUpdateCandidateAsync(candidateModel));
        }

        [Fact]
        public async Task CreateOrUpdateCandidateAsync_ShouldNotCreate_WhenInvalidModel()
        {
            var invalidCandidateModel = new CandidateModel
            {
                FirstName = null, // Invalid data
                LastName = "tester",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Comment = "Test Comment"
            };

            _candidateRepoMock.Setup(repo => repo.GetByEmailAsync(invalidCandidateModel.Email)).ReturnsAsync((CandidateProfile)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await _service.CreateOrUpdateCandidateAsync(invalidCandidateModel));

            // Assert that the exception is specifically for FirstName
            Assert.Equal("FirstName is required (Parameter 'FirstName')", exception.Message);

        }

        [Fact]
        public async Task CreateOrUpdateCandidateAsync_ShouldThrowException_WhenDuplicateEmailDuringUpdate()
        {
            // Arrange
            var candidateModel = new CandidateModel
            {
                CandidateId = 1, // ID of the candidate we want to update
                FirstName = "test",
                LastName = "tester",
                Email = "test@example.com", // Duplicate email
                PhoneNumber = "1234567890",
                Comment = "Test Comment"
            };

            // Simulate that the candidate already exists in the database
            var existingCandidate = new CandidateProfile
            {
                CandidateId = 1, 
                Email = candidateModel.Email
            };

            var otherCandidate = new CandidateProfile
            {
                CandidateId = 2,
                // Another candidate with the same email (simulating a conflict)
                Email = candidateModel.Email
            };

            _candidateRepoMock.Setup(repo => repo.GetByEmailAsync(candidateModel.Email))
                .ReturnsAsync(otherCandidate); // Simulate that there is another candidate with the same email

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _service.CreateOrUpdateCandidateAsync(candidateModel));

            // Assert that the exception is related to a duplicate email
            Assert.Equal("Candidate with this email already exists.", exception.Message);

        }
    }
}
