using Microsoft.AspNetCore.Mvc;
using Moq;
using QuizApp.API.Controllers;
using QuizApp.Application.DTOs.Requests.QuestionRepo.Read;
using QuizApp.Application.DTOs.Requests.QuestionRepo.Write;
using QuizApp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QuizApp.Tests.Controllers
{
    public class QuestionRepoControllerTests
    {
        private readonly Mock<IQuestionRepoService> _mockQuestionRepoService;
        private readonly QuestionRepoController _controller;

        public QuestionRepoControllerTests()
        {
            _mockQuestionRepoService = new Mock<IQuestionRepoService>();
            _controller = new QuestionRepoController(_mockQuestionRepoService.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new CreateQuestionRepoRequest
            {
                Title = "Test Question Repo",
                Description = "Test Description",
                CategoryId = Guid.NewGuid()
            };
            _mockQuestionRepoService.Setup(x => x.CreateAsync(request)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreateAsync(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("QuestionRepo created successfully.", okResult.Value);
        }

        [Fact]
        public async Task CreateAsync_InvalidRequest_ReturnsInternalServerError()
        {
            // Arrange
            var request = new CreateQuestionRepoRequest();
            _mockQuestionRepoService.Setup(x => x.CreateAsync(request)).ReturnsAsync(false);

            // Act
            var result = await _controller.CreateAsync(request);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void Delete_ExistingQuestionRepo_ReturnsOkResult()
        {
            // Arrange
            var questionRepoId = Guid.NewGuid();
            _mockQuestionRepoService.Setup(x => x.Delete(questionRepoId)).Returns(true);

            // Act
            var result = _controller.Delete(questionRepoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("QuestionRepo deleted successfully.", okResult.Value);
        }

        [Fact]
        public void Delete_NonExistingQuestionRepo_ReturnsNotFound()
        {
            // Arrange
            var questionRepoId = Guid.NewGuid();
            _mockQuestionRepoService.Setup(x => x.Delete(questionRepoId)).Returns(false);

            // Act
            var result = _controller.Delete(questionRepoId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("QuestionRepo not found.", notFoundResult.Value);
        }

        [Fact]
        public void Update_ExistingQuestionRepo_ReturnsOkResult()
        {
            // Arrange
            var request = new UpdateQuestionRepoRequest
            {
                Id = Guid.NewGuid(),
                Title = "Updated Question Repo",
                Description = "Updated Description"
            };
            var expectedQuestionRepo = new QuestionRepoDTO
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description
            };
            _mockQuestionRepoService.Setup(x => x.Update(request)).Returns(expectedQuestionRepo);

            // Act
            var result = _controller.Update(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuestionRepo = Assert.IsType<QuestionRepoDTO>(okResult.Value);
            Assert.Equal(expectedQuestionRepo.Id, returnedQuestionRepo.Id);
            Assert.Equal(expectedQuestionRepo.Title, returnedQuestionRepo.Title);
            Assert.Equal(expectedQuestionRepo.Description, returnedQuestionRepo.Description);
        }

        [Fact]
        public void Update_NonExistingQuestionRepo_ReturnsNotFound()
        {
            // Arrange
            var request = new UpdateQuestionRepoRequest { Id = Guid.NewGuid() };
            _mockQuestionRepoService.Setup(x => x.Update(request)).Returns((QuestionRepoDTO)null);

            // Act
            var result = _controller.Update(request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("QuestionRepo not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetById_ExistingQuestionRepo_ReturnsOkResult()
        {
            // Arrange
            var questionRepoId = Guid.NewGuid();
            var expectedQuestionRepo = new QuestionRepoDTO { Id = questionRepoId };
            _mockQuestionRepoService.Setup(x => x.GetById(questionRepoId)).Returns(expectedQuestionRepo);

            // Act
            var result = _controller.GetById(questionRepoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuestionRepo = Assert.IsType<QuestionRepoDTO>(okResult.Value);
            Assert.Equal(expectedQuestionRepo.Id, returnedQuestionRepo.Id);
        }

        [Fact]
        public void GetById_NonExistingQuestionRepo_ReturnsNotFound()
        {
            // Arrange
            var questionRepoId = Guid.NewGuid();
            _mockQuestionRepoService.Setup(x => x.GetById(questionRepoId)).Returns((QuestionRepoDTO)null);

            // Act
            var result = _controller.GetById(questionRepoId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("QuestionRepo not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetAll_ExistingQuestionRepos_ReturnsOkResult()
        {
            // Arrange
            var questionRepos = new List<QuestionRepoDTO>
            {
                new QuestionRepoDTO { Id = Guid.NewGuid() },
                new QuestionRepoDTO { Id = Guid.NewGuid() }
            };
            _mockQuestionRepoService.Setup(x => x.GetAll()).Returns(questionRepos);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuestionRepos = Assert.IsAssignableFrom<IEnumerable<QuestionRepoDTO>>(okResult.Value);
            Assert.Equal(2, returnedQuestionRepos.Count());
        }

        [Fact]
        public void GetAll_NoQuestionRepos_ReturnsNotFound()
        {
            // Arrange
            _mockQuestionRepoService.Setup(x => x.GetAll()).Returns(new List<QuestionRepoDTO>());

            // Act
            var result = _controller.GetAll();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No question repos found.", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateRangeAsync_ValidRequests_ReturnsOkResult()
        {
            // Arrange
            var requests = new List<CreateQuestionRepoRequest>
            {
                new CreateQuestionRepoRequest { Title = "Question Repo 1" },
                new CreateQuestionRepoRequest { Title = "Question Repo 2" }
            };
            var createdQuestionRepos = new List<QuestionRepoDTO>
            {
                new QuestionRepoDTO { Id = Guid.NewGuid() },
                new QuestionRepoDTO { Id = Guid.NewGuid() }
            };
            _mockQuestionRepoService.Setup(x => x.CreateRangeAsync(requests)).ReturnsAsync(createdQuestionRepos);

            // Act
            var result = await _controller.CreateRangeAsync(requests);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuestionRepos = Assert.IsAssignableFrom<IEnumerable<QuestionRepoDTO>>(okResult.Value);
            Assert.Equal(2, returnedQuestionRepos.Count());
        }

        [Fact]
        public void DeleteRange_ExistingQuestionRepos_ReturnsOkResult()
        {
            // Arrange
            var questionRepoIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockQuestionRepoService.Setup(x => x.DeleteRange(questionRepoIds)).Returns(true);

            // Act
            var result = _controller.DeleteRange(questionRepoIds);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("QuestionRepos deleted successfully.", okResult.Value);
        }

        [Fact]
        public void DeleteRange_NonExistingQuestionRepos_ReturnsNotFound()
        {
            // Arrange
            var questionRepoIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockQuestionRepoService.Setup(x => x.DeleteRange(questionRepoIds)).Returns(false);

            // Act
            var result = _controller.DeleteRange(questionRepoIds);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No question repos found for the provided IDs.", notFoundResult.Value);
        }
    }
} 