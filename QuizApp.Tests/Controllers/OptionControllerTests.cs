using Microsoft.AspNetCore.Mvc;
using Moq;
using QuizApp.API.Controllers;
using QuizApp.Application.DTOs.Requests.Option.Read;
using QuizApp.Application.DTOs.Requests.Option.Write;
using QuizApp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QuizApp.Tests.Controllers
{
    public class OptionControllerTests
    {
        private readonly Mock<IOptionService> _mockOptionService;
        private readonly OptionController _controller;

        public OptionControllerTests()
        {
            _mockOptionService = new Mock<IOptionService>();
            _controller = new OptionController(_mockOptionService.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new CreateOptionRequest
            {
                QuestionId = Guid.NewGuid(),
                Text = "Test Option",
                IsCorrect = true
            };
            _mockOptionService.Setup(x => x.CreateAsync(request)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreateAsync(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Option created successfully.", okResult.Value);
        }

        [Fact]
        public async Task CreateAsync_InvalidRequest_ReturnsInternalServerError()
        {
            // Arrange
            var request = new CreateOptionRequest();
            _mockOptionService.Setup(x => x.CreateAsync(request)).ReturnsAsync(false);

            // Act
            var result = await _controller.CreateAsync(request);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void Delete_ExistingOption_ReturnsOkResult()
        {
            // Arrange
            var optionId = Guid.NewGuid();
            _mockOptionService.Setup(x => x.Delete(optionId)).Returns(true);

            // Act
            var result = _controller.Delete(optionId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Option deleted successfully.", okResult.Value);
        }

        [Fact]
        public void Delete_NonExistingOption_ReturnsNotFound()
        {
            // Arrange
            var optionId = Guid.NewGuid();
            _mockOptionService.Setup(x => x.Delete(optionId)).Returns(false);

            // Act
            var result = _controller.Delete(optionId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Option not found.", notFoundResult.Value);
        }

        [Fact]
        public void Update_ExistingOption_ReturnsOkResult()
        {
            // Arrange
            var request = new UpdateOptionRequest
            {
                Id = Guid.NewGuid(),
                Text = "Updated Option",
                IsCorrect = false
            };
            var expectedOption = new OptionDTO
            {
                Id = request.Id,
                Text = request.Text,
                IsCorrect = request.IsCorrect
            };
            _mockOptionService.Setup(x => x.Update(request)).Returns(expectedOption);

            // Act
            var result = _controller.Update(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOption = Assert.IsType<OptionDTO>(okResult.Value);
            Assert.Equal(expectedOption.Id, returnedOption.Id);
            Assert.Equal(expectedOption.Text, returnedOption.Text);
            Assert.Equal(expectedOption.IsCorrect, returnedOption.IsCorrect);
        }

        [Fact]
        public void Update_NonExistingOption_ReturnsNotFound()
        {
            // Arrange
            var request = new UpdateOptionRequest { Id = Guid.NewGuid() };
            _mockOptionService.Setup(x => x.Update(request)).Returns((OptionDTO)null);

            // Act
            var result = _controller.Update(request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Option not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetById_ExistingOption_ReturnsOkResult()
        {
            // Arrange
            var optionId = Guid.NewGuid();
            var expectedOption = new OptionDTO { Id = optionId };
            _mockOptionService.Setup(x => x.GetById(optionId)).Returns(expectedOption);

            // Act
            var result = _controller.GetById(optionId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOption = Assert.IsType<OptionDTO>(okResult.Value);
            Assert.Equal(expectedOption.Id, returnedOption.Id);
        }

        [Fact]
        public void GetById_NonExistingOption_ReturnsNotFound()
        {
            // Arrange
            var optionId = Guid.NewGuid();
            _mockOptionService.Setup(x => x.GetById(optionId)).Returns((OptionDTO)null);

            // Act
            var result = _controller.GetById(optionId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Option not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetAll_ExistingOptions_ReturnsOkResult()
        {
            // Arrange
            var options = new List<OptionDTO>
            {
                new OptionDTO { Id = Guid.NewGuid() },
                new OptionDTO { Id = Guid.NewGuid() }
            };
            _mockOptionService.Setup(x => x.GetAll()).Returns(options);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOptions = Assert.IsAssignableFrom<IEnumerable<OptionDTO>>(okResult.Value);
            Assert.Equal(2, returnedOptions.Count());
        }

        [Fact]
        public void GetAll_NoOptions_ReturnsNotFound()
        {
            // Arrange
            _mockOptionService.Setup(x => x.GetAll()).Returns(new List<OptionDTO>());

            // Act
            var result = _controller.GetAll();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No options found.", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateRangeAsync_ValidRequests_ReturnsOkResult()
        {
            // Arrange
            var requests = new List<CreateOptionRequest>
            {
                new CreateOptionRequest { Text = "Option 1" },
                new CreateOptionRequest { Text = "Option 2" }
            };
            var createdOptions = new List<OptionDTO>
            {
                new OptionDTO { Id = Guid.NewGuid() },
                new OptionDTO { Id = Guid.NewGuid() }
            };
            _mockOptionService.Setup(x => x.CreateRangeAsync(requests)).ReturnsAsync(createdOptions);

            // Act
            var result = await _controller.CreateRangeAsync(requests);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOptions = Assert.IsAssignableFrom<IEnumerable<OptionDTO>>(okResult.Value);
            Assert.Equal(2, returnedOptions.Count());
        }

        [Fact]
        public void DeleteRange_ExistingOptions_ReturnsOkResult()
        {
            // Arrange
            var optionIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockOptionService.Setup(x => x.DeleteRange(optionIds)).Returns(true);

            // Act
            var result = _controller.DeleteRange(optionIds);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Options deleted successfully.", okResult.Value);
        }

        [Fact]
        public void DeleteRange_NonExistingOptions_ReturnsNotFound()
        {
            // Arrange
            var optionIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockOptionService.Setup(x => x.DeleteRange(optionIds)).Returns(false);

            // Act
            var result = _controller.DeleteRange(optionIds);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No options found for the provided IDs.", notFoundResult.Value);
        }
    }
} 