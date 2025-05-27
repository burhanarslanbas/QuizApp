using Microsoft.AspNetCore.Mvc;
using Moq;
using QuizApp.API.Controllers;
using QuizApp.Application.DTOs.Requests.QuizResult.Read;
using QuizApp.Application.DTOs.Requests.QuizResult.Write;
using QuizApp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QuizApp.Tests.Controllers
{
    public class QuizResultControllerTests
    {
        private readonly Mock<IQuizResultService> _mockQuizResultService;
        private readonly QuizResultController _controller;

        public QuizResultControllerTests()
        {
            _mockQuizResultService = new Mock<IQuizResultService>();
            _controller = new QuizResultController(_mockQuizResultService.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new CreateQuizResultRequest
            {
                UserId = Guid.NewGuid(),
                QuizId = Guid.NewGuid(),
                Score = 85,
                TimeSpent = 25
            };
            _mockQuizResultService.Setup(x => x.CreateAsync(request)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreateAsync(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("QuizResult created successfully.", okResult.Value);
        }

        [Fact]
        public async Task CreateAsync_InvalidRequest_ReturnsInternalServerError()
        {
            // Arrange
            var request = new CreateQuizResultRequest();
            _mockQuizResultService.Setup(x => x.CreateAsync(request)).ReturnsAsync(false);

            // Act
            var result = await _controller.CreateAsync(request);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void Delete_ExistingQuizResult_ReturnsOkResult()
        {
            // Arrange
            var quizResultId = Guid.NewGuid();
            _mockQuizResultService.Setup(x => x.Delete(quizResultId)).Returns(true);

            // Act
            var result = _controller.Delete(quizResultId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("QuizResult deleted successfully.", okResult.Value);
        }

        [Fact]
        public void Delete_NonExistingQuizResult_ReturnsNotFound()
        {
            // Arrange
            var quizResultId = Guid.NewGuid();
            _mockQuizResultService.Setup(x => x.Delete(quizResultId)).Returns(false);

            // Act
            var result = _controller.Delete(quizResultId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("QuizResult not found.", notFoundResult.Value);
        }

        [Fact]
        public void Update_ExistingQuizResult_ReturnsOkResult()
        {
            // Arrange
            var request = new UpdateQuizResultRequest
            {
                Id = Guid.NewGuid(),
                Score = 90,
                TimeSpent = 28
            };
            var expectedQuizResult = new QuizResultDTO
            {
                Id = request.Id,
                Score = request.Score,
                TimeSpent = request.TimeSpent
            };
            _mockQuizResultService.Setup(x => x.Update(request)).Returns(expectedQuizResult);

            // Act
            var result = _controller.Update(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuizResult = Assert.IsType<QuizResultDTO>(okResult.Value);
            Assert.Equal(expectedQuizResult.Id, returnedQuizResult.Id);
            Assert.Equal(expectedQuizResult.Score, returnedQuizResult.Score);
            Assert.Equal(expectedQuizResult.TimeSpent, returnedQuizResult.TimeSpent);
        }

        [Fact]
        public void Update_NonExistingQuizResult_ReturnsNotFound()
        {
            // Arrange
            var request = new UpdateQuizResultRequest { Id = Guid.NewGuid() };
            _mockQuizResultService.Setup(x => x.Update(request)).Returns((QuizResultDTO)null);

            // Act
            var result = _controller.Update(request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("QuizResult not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetById_ExistingQuizResult_ReturnsOkResult()
        {
            // Arrange
            var quizResultId = Guid.NewGuid();
            var expectedQuizResult = new QuizResultDTO { Id = quizResultId };
            _mockQuizResultService.Setup(x => x.GetById(quizResultId)).Returns(expectedQuizResult);

            // Act
            var result = _controller.GetById(quizResultId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuizResult = Assert.IsType<QuizResultDTO>(okResult.Value);
            Assert.Equal(expectedQuizResult.Id, returnedQuizResult.Id);
        }

        [Fact]
        public void GetById_NonExistingQuizResult_ReturnsNotFound()
        {
            // Arrange
            var quizResultId = Guid.NewGuid();
            _mockQuizResultService.Setup(x => x.GetById(quizResultId)).Returns((QuizResultDTO)null);

            // Act
            var result = _controller.GetById(quizResultId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("QuizResult not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetAll_ExistingQuizResults_ReturnsOkResult()
        {
            // Arrange
            var quizResults = new List<QuizResultDTO>
            {
                new QuizResultDTO { Id = Guid.NewGuid() },
                new QuizResultDTO { Id = Guid.NewGuid() }
            };
            _mockQuizResultService.Setup(x => x.GetAll()).Returns(quizResults);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuizResults = Assert.IsAssignableFrom<IEnumerable<QuizResultDTO>>(okResult.Value);
            Assert.Equal(2, returnedQuizResults.Count());
        }

        [Fact]
        public void GetAll_NoQuizResults_ReturnsNotFound()
        {
            // Arrange
            _mockQuizResultService.Setup(x => x.GetAll()).Returns(new List<QuizResultDTO>());

            // Act
            var result = _controller.GetAll();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No quiz results found.", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateRangeAsync_ValidRequests_ReturnsOkResult()
        {
            // Arrange
            var requests = new List<CreateQuizResultRequest>
            {
                new CreateQuizResultRequest { Score = 85 },
                new CreateQuizResultRequest { Score = 90 }
            };
            var createdQuizResults = new List<QuizResultDTO>
            {
                new QuizResultDTO { Id = Guid.NewGuid() },
                new QuizResultDTO { Id = Guid.NewGuid() }
            };
            _mockQuizResultService.Setup(x => x.CreateRangeAsync(requests)).ReturnsAsync(createdQuizResults);

            // Act
            var result = await _controller.CreateRangeAsync(requests);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuizResults = Assert.IsAssignableFrom<IEnumerable<QuizResultDTO>>(okResult.Value);
            Assert.Equal(2, returnedQuizResults.Count());
        }

        [Fact]
        public void DeleteRange_ExistingQuizResults_ReturnsOkResult()
        {
            // Arrange
            var quizResultIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockQuizResultService.Setup(x => x.DeleteRange(quizResultIds)).Returns(true);

            // Act
            var result = _controller.DeleteRange(quizResultIds);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("QuizResults deleted successfully.", okResult.Value);
        }

        [Fact]
        public void DeleteRange_NonExistingQuizResults_ReturnsNotFound()
        {
            // Arrange
            var quizResultIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockQuizResultService.Setup(x => x.DeleteRange(quizResultIds)).Returns(false);

            // Act
            var result = _controller.DeleteRange(quizResultIds);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No quiz results found for the provided IDs.", notFoundResult.Value);
        }
    }
} 