using Microsoft.AspNetCore.Mvc;
using Moq;
using QuizApp.API.Controllers;
using QuizApp.Application.DTOs.Requests.UserAnswer.Read;
using QuizApp.Application.DTOs.Requests.UserAnswer.Write;
using QuizApp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QuizApp.Tests.Controllers
{
    public class UserAnswerControllerTests
    {
        private readonly Mock<IUserAnswerService> _mockUserAnswerService;
        private readonly UserAnswerController _controller;

        public UserAnswerControllerTests()
        {
            _mockUserAnswerService = new Mock<IUserAnswerService>();
            _controller = new UserAnswerController(_mockUserAnswerService.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new CreateUserAnswerRequest
            {
                UserId = Guid.NewGuid(),
                QuestionId = Guid.NewGuid(),
                OptionId = Guid.NewGuid(),
                QuizResultId = Guid.NewGuid()
            };
            _mockUserAnswerService.Setup(x => x.CreateAsync(request)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreateAsync(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("UserAnswer created successfully.", okResult.Value);
        }

        [Fact]
        public async Task CreateAsync_InvalidRequest_ReturnsInternalServerError()
        {
            // Arrange
            var request = new CreateUserAnswerRequest();
            _mockUserAnswerService.Setup(x => x.CreateAsync(request)).ReturnsAsync(false);

            // Act
            var result = await _controller.CreateAsync(request);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void Delete_ExistingUserAnswer_ReturnsOkResult()
        {
            // Arrange
            var userAnswerId = Guid.NewGuid();
            _mockUserAnswerService.Setup(x => x.Delete(userAnswerId)).Returns(true);

            // Act
            var result = _controller.Delete(userAnswerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("UserAnswer deleted successfully.", okResult.Value);
        }

        [Fact]
        public void Delete_NonExistingUserAnswer_ReturnsNotFound()
        {
            // Arrange
            var userAnswerId = Guid.NewGuid();
            _mockUserAnswerService.Setup(x => x.Delete(userAnswerId)).Returns(false);

            // Act
            var result = _controller.Delete(userAnswerId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("UserAnswer not found.", notFoundResult.Value);
        }

        [Fact]
        public void Update_ExistingUserAnswer_ReturnsOkResult()
        {
            // Arrange
            var request = new UpdateUserAnswerRequest
            {
                Id = Guid.NewGuid(),
                OptionId = Guid.NewGuid()
            };
            var expectedUserAnswer = new UserAnswerDTO
            {
                Id = request.Id,
                OptionId = request.OptionId
            };
            _mockUserAnswerService.Setup(x => x.Update(request)).Returns(expectedUserAnswer);

            // Act
            var result = _controller.Update(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUserAnswer = Assert.IsType<UserAnswerDTO>(okResult.Value);
            Assert.Equal(expectedUserAnswer.Id, returnedUserAnswer.Id);
            Assert.Equal(expectedUserAnswer.OptionId, returnedUserAnswer.OptionId);
        }

        [Fact]
        public void Update_NonExistingUserAnswer_ReturnsNotFound()
        {
            // Arrange
            var request = new UpdateUserAnswerRequest { Id = Guid.NewGuid() };
            _mockUserAnswerService.Setup(x => x.Update(request)).Returns((UserAnswerDTO)null);

            // Act
            var result = _controller.Update(request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("UserAnswer not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetById_ExistingUserAnswer_ReturnsOkResult()
        {
            // Arrange
            var userAnswerId = Guid.NewGuid();
            var expectedUserAnswer = new UserAnswerDTO { Id = userAnswerId };
            _mockUserAnswerService.Setup(x => x.GetById(userAnswerId)).Returns(expectedUserAnswer);

            // Act
            var result = _controller.GetById(userAnswerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUserAnswer = Assert.IsType<UserAnswerDTO>(okResult.Value);
            Assert.Equal(expectedUserAnswer.Id, returnedUserAnswer.Id);
        }

        [Fact]
        public void GetById_NonExistingUserAnswer_ReturnsNotFound()
        {
            // Arrange
            var userAnswerId = Guid.NewGuid();
            _mockUserAnswerService.Setup(x => x.GetById(userAnswerId)).Returns((UserAnswerDTO)null);

            // Act
            var result = _controller.GetById(userAnswerId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("UserAnswer not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetAll_ExistingUserAnswers_ReturnsOkResult()
        {
            // Arrange
            var userAnswers = new List<UserAnswerDTO>
            {
                new UserAnswerDTO { Id = Guid.NewGuid() },
                new UserAnswerDTO { Id = Guid.NewGuid() }
            };
            _mockUserAnswerService.Setup(x => x.GetAll()).Returns(userAnswers);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUserAnswers = Assert.IsAssignableFrom<IEnumerable<UserAnswerDTO>>(okResult.Value);
            Assert.Equal(2, returnedUserAnswers.Count());
        }

        [Fact]
        public void GetAll_NoUserAnswers_ReturnsNotFound()
        {
            // Arrange
            _mockUserAnswerService.Setup(x => x.GetAll()).Returns(new List<UserAnswerDTO>());

            // Act
            var result = _controller.GetAll();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No user answers found.", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateRangeAsync_ValidRequests_ReturnsOkResult()
        {
            // Arrange
            var requests = new List<CreateUserAnswerRequest>
            {
                new CreateUserAnswerRequest { OptionId = Guid.NewGuid() },
                new CreateUserAnswerRequest { OptionId = Guid.NewGuid() }
            };
            var createdUserAnswers = new List<UserAnswerDTO>
            {
                new UserAnswerDTO { Id = Guid.NewGuid() },
                new UserAnswerDTO { Id = Guid.NewGuid() }
            };
            _mockUserAnswerService.Setup(x => x.CreateRangeAsync(requests)).ReturnsAsync(createdUserAnswers);

            // Act
            var result = await _controller.CreateRangeAsync(requests);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUserAnswers = Assert.IsAssignableFrom<IEnumerable<UserAnswerDTO>>(okResult.Value);
            Assert.Equal(2, returnedUserAnswers.Count());
        }

        [Fact]
        public void DeleteRange_ExistingUserAnswers_ReturnsOkResult()
        {
            // Arrange
            var userAnswerIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockUserAnswerService.Setup(x => x.DeleteRange(userAnswerIds)).Returns(true);

            // Act
            var result = _controller.DeleteRange(userAnswerIds);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("UserAnswers deleted successfully.", okResult.Value);
        }

        [Fact]
        public void DeleteRange_NonExistingUserAnswers_ReturnsNotFound()
        {
            // Arrange
            var userAnswerIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockUserAnswerService.Setup(x => x.DeleteRange(userAnswerIds)).Returns(false);

            // Act
            var result = _controller.DeleteRange(userAnswerIds);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No user answers found for the provided IDs.", notFoundResult.Value);
        }
    }
} 