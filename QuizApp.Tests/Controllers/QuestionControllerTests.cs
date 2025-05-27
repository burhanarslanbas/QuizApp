using Microsoft.AspNetCore.Mvc;
using Moq;
using QuizApp.API.Controllers;
using QuizApp.Application.DTOs.Requests.Question.Read;
using QuizApp.Application.DTOs.Requests.Question.Write;
using QuizApp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QuizApp.Tests.Controllers
{
    public class QuestionControllerTests
    {
        private readonly Mock<IQuestionService> _mockQuestionService;
        private readonly QuestionController _controller;

        public QuestionControllerTests()
        {
            _mockQuestionService = new Mock<IQuestionService>();
            _controller = new QuestionController(_mockQuestionService.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new CreateQuestionRequest
            {
                QuizId = Guid.NewGuid(),
                Text = "Test Question",
                Points = 10,
                Type = "MultipleChoice"
            };
            _mockQuestionService.Setup(x => x.CreateAsync(request)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreateAsync(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Question created successfully.", okResult.Value);
        }

        [Fact]
        public async Task CreateAsync_InvalidRequest_ReturnsInternalServerError()
        {
            // Arrange
            var request = new CreateQuestionRequest();
            _mockQuestionService.Setup(x => x.CreateAsync(request)).ReturnsAsync(false);

            // Act
            var result = await _controller.CreateAsync(request);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void Delete_ExistingQuestion_ReturnsOkResult()
        {
            // Arrange
            var questionId = Guid.NewGuid();
            _mockQuestionService.Setup(x => x.Delete(questionId)).Returns(true);

            // Act
            var result = _controller.Delete(questionId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Question deleted successfully.", okResult.Value);
        }

        [Fact]
        public void Delete_NonExistingQuestion_ReturnsNotFound()
        {
            // Arrange
            var questionId = Guid.NewGuid();
            _mockQuestionService.Setup(x => x.Delete(questionId)).Returns(false);

            // Act
            var result = _controller.Delete(questionId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Question not found.", notFoundResult.Value);
        }

        [Fact]
        public void Update_ExistingQuestion_ReturnsOkResult()
        {
            // Arrange
            var request = new UpdateQuestionRequest
            {
                Id = Guid.NewGuid(),
                Text = "Updated Question",
                Points = 20
            };
            var expectedQuestion = new QuestionDTO
            {
                Id = request.Id,
                Text = request.Text,
                Points = request.Points
            };
            _mockQuestionService.Setup(x => x.Update(request)).Returns(expectedQuestion);

            // Act
            var result = _controller.Update(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuestion = Assert.IsType<QuestionDTO>(okResult.Value);
            Assert.Equal(expectedQuestion.Id, returnedQuestion.Id);
            Assert.Equal(expectedQuestion.Text, returnedQuestion.Text);
            Assert.Equal(expectedQuestion.Points, returnedQuestion.Points);
        }

        [Fact]
        public void Update_NonExistingQuestion_ReturnsNotFound()
        {
            // Arrange
            var request = new UpdateQuestionRequest { Id = Guid.NewGuid() };
            _mockQuestionService.Setup(x => x.Update(request)).Returns((QuestionDTO)null);

            // Act
            var result = _controller.Update(request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Question not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetById_ExistingQuestion_ReturnsOkResult()
        {
            // Arrange
            var questionId = Guid.NewGuid();
            var expectedQuestion = new QuestionDTO { Id = questionId };
            _mockQuestionService.Setup(x => x.GetById(questionId)).Returns(expectedQuestion);

            // Act
            var result = _controller.GetById(questionId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuestion = Assert.IsType<QuestionDTO>(okResult.Value);
            Assert.Equal(expectedQuestion.Id, returnedQuestion.Id);
        }

        [Fact]
        public void GetById_NonExistingQuestion_ReturnsNotFound()
        {
            // Arrange
            var questionId = Guid.NewGuid();
            _mockQuestionService.Setup(x => x.GetById(questionId)).Returns((QuestionDTO)null);

            // Act
            var result = _controller.GetById(questionId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Question not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetAll_ExistingQuestions_ReturnsOkResult()
        {
            // Arrange
            var questions = new List<QuestionDTO>
            {
                new QuestionDTO { Id = Guid.NewGuid() },
                new QuestionDTO { Id = Guid.NewGuid() }
            };
            _mockQuestionService.Setup(x => x.GetAll()).Returns(questions);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuestions = Assert.IsAssignableFrom<IEnumerable<QuestionDTO>>(okResult.Value);
            Assert.Equal(2, returnedQuestions.Count());
        }

        [Fact]
        public void GetAll_NoQuestions_ReturnsNotFound()
        {
            // Arrange
            _mockQuestionService.Setup(x => x.GetAll()).Returns(new List<QuestionDTO>());

            // Act
            var result = _controller.GetAll();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No questions found.", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateRangeAsync_ValidRequests_ReturnsOkResult()
        {
            // Arrange
            var requests = new List<CreateQuestionRequest>
            {
                new CreateQuestionRequest { Text = "Question 1" },
                new CreateQuestionRequest { Text = "Question 2" }
            };
            var createdQuestions = new List<QuestionDTO>
            {
                new QuestionDTO { Id = Guid.NewGuid() },
                new QuestionDTO { Id = Guid.NewGuid() }
            };
            _mockQuestionService.Setup(x => x.CreateRangeAsync(requests)).ReturnsAsync(createdQuestions);

            // Act
            var result = await _controller.CreateRangeAsync(requests);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuestions = Assert.IsAssignableFrom<IEnumerable<QuestionDTO>>(okResult.Value);
            Assert.Equal(2, returnedQuestions.Count());
        }

        [Fact]
        public void DeleteRange_ExistingQuestions_ReturnsOkResult()
        {
            // Arrange
            var questionIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockQuestionService.Setup(x => x.DeleteRange(questionIds)).Returns(true);

            // Act
            var result = _controller.DeleteRange(questionIds);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Questions deleted successfully.", okResult.Value);
        }

        [Fact]
        public void DeleteRange_NonExistingQuestions_ReturnsNotFound()
        {
            // Arrange
            var questionIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockQuestionService.Setup(x => x.DeleteRange(questionIds)).Returns(false);

            // Act
            var result = _controller.DeleteRange(questionIds);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No questions found for the provided IDs.", notFoundResult.Value);
        }
    }
} 