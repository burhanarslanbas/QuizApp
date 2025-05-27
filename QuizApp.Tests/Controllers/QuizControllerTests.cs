using Microsoft.AspNetCore.Mvc;
using Moq;
using QuizApp.API.Controllers;
using QuizApp.Application.DTOs.Requests.Quiz.Read;
using QuizApp.Application.DTOs.Requests.Quiz.Write;
using QuizApp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QuizApp.Tests.Controllers
{
    public class QuizControllerTests
    {
        private readonly Mock<IQuizService> _mockQuizService;
        private readonly QuizController _controller;

        public QuizControllerTests()
        {
            _mockQuizService = new Mock<IQuizService>();
            _controller = new QuizController(_mockQuizService.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new CreateQuizRequest
            {
                Title = "Test Quiz",
                Description = "Test Description",
                CategoryId = Guid.NewGuid(),
                TimeLimit = 30
            };
            _mockQuizService.Setup(x => x.CreateAsync(request)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreateAsync(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Quiz created successfully.", okResult.Value);
        }

        [Fact]
        public async Task CreateAsync_InvalidRequest_ReturnsInternalServerError()
        {
            // Arrange
            var request = new CreateQuizRequest();
            _mockQuizService.Setup(x => x.CreateAsync(request)).ReturnsAsync(false);

            // Act
            var result = await _controller.CreateAsync(request);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void Delete_ExistingQuiz_ReturnsOkResult()
        {
            // Arrange
            var quizId = Guid.NewGuid();
            _mockQuizService.Setup(x => x.Delete(quizId)).Returns(true);

            // Act
            var result = _controller.Delete(quizId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Quiz deleted successfully.", okResult.Value);
        }

        [Fact]
        public void Delete_NonExistingQuiz_ReturnsNotFound()
        {
            // Arrange
            var quizId = Guid.NewGuid();
            _mockQuizService.Setup(x => x.Delete(quizId)).Returns(false);

            // Act
            var result = _controller.Delete(quizId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Quiz not found.", notFoundResult.Value);
        }

        [Fact]
        public void Update_ExistingQuiz_ReturnsOkResult()
        {
            // Arrange
            var request = new UpdateQuizRequest
            {
                Id = Guid.NewGuid(),
                Title = "Updated Quiz",
                Description = "Updated Description"
            };
            var expectedQuiz = new QuizDTO
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description
            };
            _mockQuizService.Setup(x => x.Update(request)).Returns(expectedQuiz);

            // Act
            var result = _controller.Update(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuiz = Assert.IsType<QuizDTO>(okResult.Value);
            Assert.Equal(expectedQuiz.Id, returnedQuiz.Id);
            Assert.Equal(expectedQuiz.Title, returnedQuiz.Title);
            Assert.Equal(expectedQuiz.Description, returnedQuiz.Description);
        }

        [Fact]
        public void Update_NonExistingQuiz_ReturnsNotFound()
        {
            // Arrange
            var request = new UpdateQuizRequest { Id = Guid.NewGuid() };
            _mockQuizService.Setup(x => x.Update(request)).Returns((QuizDTO)null);

            // Act
            var result = _controller.Update(request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Quiz not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetById_ExistingQuiz_ReturnsOkResult()
        {
            // Arrange
            var quizId = Guid.NewGuid();
            var expectedQuiz = new QuizDTO { Id = quizId };
            _mockQuizService.Setup(x => x.GetById(quizId)).Returns(expectedQuiz);

            // Act
            var result = _controller.GetById(quizId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuiz = Assert.IsType<QuizDTO>(okResult.Value);
            Assert.Equal(expectedQuiz.Id, returnedQuiz.Id);
        }

        [Fact]
        public void GetById_NonExistingQuiz_ReturnsNotFound()
        {
            // Arrange
            var quizId = Guid.NewGuid();
            _mockQuizService.Setup(x => x.GetById(quizId)).Returns((QuizDTO)null);

            // Act
            var result = _controller.GetById(quizId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Quiz not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetAll_ExistingQuizzes_ReturnsOkResult()
        {
            // Arrange
            var quizzes = new List<QuizDTO>
            {
                new QuizDTO { Id = Guid.NewGuid() },
                new QuizDTO { Id = Guid.NewGuid() }
            };
            _mockQuizService.Setup(x => x.GetAll()).Returns(quizzes);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuizzes = Assert.IsAssignableFrom<IEnumerable<QuizDTO>>(okResult.Value);
            Assert.Equal(2, returnedQuizzes.Count());
        }

        [Fact]
        public void GetAll_NoQuizzes_ReturnsNotFound()
        {
            // Arrange
            _mockQuizService.Setup(x => x.GetAll()).Returns(new List<QuizDTO>());

            // Act
            var result = _controller.GetAll();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No quizzes found.", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateRangeAsync_ValidRequests_ReturnsOkResult()
        {
            // Arrange
            var requests = new List<CreateQuizRequest>
            {
                new CreateQuizRequest { Title = "Quiz 1" },
                new CreateQuizRequest { Title = "Quiz 2" }
            };
            var createdQuizzes = new List<QuizDTO>
            {
                new QuizDTO { Id = Guid.NewGuid() },
                new QuizDTO { Id = Guid.NewGuid() }
            };
            _mockQuizService.Setup(x => x.CreateRangeAsync(requests)).ReturnsAsync(createdQuizzes);

            // Act
            var result = await _controller.CreateRangeAsync(requests);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuizzes = Assert.IsAssignableFrom<IEnumerable<QuizDTO>>(okResult.Value);
            Assert.Equal(2, returnedQuizzes.Count());
        }

        [Fact]
        public void DeleteRange_ExistingQuizzes_ReturnsOkResult()
        {
            // Arrange
            var quizIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockQuizService.Setup(x => x.DeleteRange(quizIds)).Returns(true);

            // Act
            var result = _controller.DeleteRange(quizIds);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Quizzes deleted successfully.", okResult.Value);
        }

        [Fact]
        public void DeleteRange_NonExistingQuizzes_ReturnsNotFound()
        {
            // Arrange
            var quizIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockQuizService.Setup(x => x.DeleteRange(quizIds)).Returns(false);

            // Act
            var result = _controller.DeleteRange(quizIds);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No quizzes found for the provided IDs.", notFoundResult.Value);
        }
    }
} 