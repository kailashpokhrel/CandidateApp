using Candidate.API.Controllers;
using Candidate.Application.Models;
using Candidate.Application.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Candidate.Test.Controllers
{
    public class CandidatesControllerTests
    {
        private readonly Mock<ICandidateService> _candidateServiceMock;
        private readonly CandidateController _controller;

        public CandidatesControllerTests()
        {
            _candidateServiceMock = new Mock<ICandidateService>();
            _controller = new CandidateController(_candidateServiceMock.Object, null);
        }

        
        [Fact]
        public async Task CreateOrUpdateCandidateAsync_ShouldReturnOk_WhenValidDataIsProvided()
        {
            // Arrange
            var candidateModel = new CandidateModel
            {
                FirstName = "test",
                LastName = "tester",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Comment = "Test Comment"
            };

            var candidateDto = new CandidateDto { Email = candidateModel.Email };

            _candidateServiceMock.Setup(service => service.CreateOrUpdateCandidateAsync(candidateModel))
                .ReturnsAsync(candidateDto);

            // Act
            var result = await _controller.CreateOrUpdateCandidateAsync(candidateModel);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(candidateDto, okResult.Value);
        }

        [Fact]
        public async Task CreateOrUpdateCandidateAsync_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange: Add model error (e.g., "FirstName" is required)
            _controller.ModelState.AddModelError("FirstName", "Required");

            var candidateModel = new CandidateModel();

            // Act
            var result = await _controller.CreateOrUpdateCandidateAsync(candidateModel);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task CreateOrUpdateCandidateAsync_ShouldReturnBadRequest_WhenEmailIsInvalid()
        {
            // Arrange: Add email error (e.g., invalid email format)
            _controller.ModelState.AddModelError("Email", "Invalid email format");

            var candidateModel = new CandidateModel
            {
                FirstName = "test",
                LastName = "tester",
                Email = "invalid-email",
                PhoneNumber = "1234567890",
                Comment = "Test Comment"
            };

            // Act
            var result = await _controller.CreateOrUpdateCandidateAsync(candidateModel);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task CreateOrUpdateCandidateAsync_ShouldReturnConflict_WhenEmailAlreadyExists()
        {
            // Arrange
            var candidateModel = new CandidateModel
            {
                FirstName = "test",
                LastName = "tester",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Comment = "Test Comment"
            };

            var existingCandidateDto = new CandidateDto { Email = candidateModel.Email };

            _candidateServiceMock.Setup(service => service.CreateOrUpdateCandidateAsync(candidateModel))
                .ThrowsAsync(new InvalidOperationException("Candidate with this email already exists"));

            // Act
            var result = await _controller.CreateOrUpdateCandidateAsync(candidateModel);

            // Assert
            var conflictResult = result as ConflictObjectResult;
            Assert.NotNull(conflictResult);
            Assert.Equal(409, conflictResult.StatusCode);
        }

        [Fact]
        public async Task CreateOrUpdateCandidateAsync_ShouldReturnNotFound_WhenServiceReturnsNull()
        {
            // Arrange
            var candidateModel = new CandidateModel
            {
                FirstName = "test",
                LastName = "tester",
                Email = "nonexistent@example.com",
                PhoneNumber = "1234567890",
                Comment = "Test Comment"
            };

            _candidateServiceMock.Setup(service => service.CreateOrUpdateCandidateAsync(candidateModel))
                .ReturnsAsync((CandidateDto)null);  // Simulating no candidate found

            // Act
            var result = await _controller.CreateOrUpdateCandidateAsync(candidateModel);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task CreateOrUpdateCandidateAsync_ShouldReturnServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var candidateModel = new CandidateModel
            {
                FirstName = "test",
                LastName = "tester",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Comment = "Test Comment"
            };

            _candidateServiceMock.Setup(service => service.CreateOrUpdateCandidateAsync(candidateModel))
                .ThrowsAsync(new System.Exception("Internal error"));

            // Act
            var result = await _controller.CreateOrUpdateCandidateAsync(candidateModel);

            // Assert
            var statusCodeResult = result as ObjectResult;
            Assert.NotNull(statusCodeResult);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

    }
}
