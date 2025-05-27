using Microsoft.AspNetCore.Mvc;
using Moq;
using QuizApp.API.Controllers;
using QuizApp.Application.DTOs.Requests.User.Read;
using QuizApp.Application.DTOs.Requests.User.Write;
using QuizApp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QuizApp.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                Username = "testuser",
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User",
                Password = "Test123!"
            };
            _mockUserService.Setup(x => x.CreateAsync(request)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreateAsync(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User created successfully.", okResult.Value);
        }

        [Fact]
        public async Task CreateAsync_InvalidRequest_ReturnsInternalServerError()
        {
            // Arrange
            var request = new CreateUserRequest();
            _mockUserService.Setup(x => x.CreateAsync(request)).ReturnsAsync(false);

            // Act
            var result = await _controller.CreateAsync(request);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void Delete_ExistingUser_ReturnsOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserService.Setup(x => x.Delete(userId)).Returns(true);

            // Act
            var result = _controller.Delete(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User deleted successfully.", okResult.Value);
        }

        [Fact]
        public void Delete_NonExistingUser_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserService.Setup(x => x.Delete(userId)).Returns(false);

            // Act
            var result = _controller.Delete(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", notFoundResult.Value);
        }

        [Fact]
        public void Update_ExistingUser_ReturnsOkResult()
        {
            // Arrange
            var request = new UpdateUserRequest
            {
                Id = Guid.NewGuid(),
                FirstName = "Updated",
                LastName = "Name"
            };
            var expectedUser = new UserDTO
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            _mockUserService.Setup(x => x.Update(request)).Returns(expectedUser);

            // Act
            var result = _controller.Update(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(expectedUser.Id, returnedUser.Id);
            Assert.Equal(expectedUser.FirstName, returnedUser.FirstName);
            Assert.Equal(expectedUser.LastName, returnedUser.LastName);
        }

        [Fact]
        public void Update_NonExistingUser_ReturnsNotFound()
        {
            // Arrange
            var request = new UpdateUserRequest { Id = Guid.NewGuid() };
            _mockUserService.Setup(x => x.Update(request)).Returns((UserDTO)null);

            // Act
            var result = _controller.Update(request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetById_ExistingUser_ReturnsOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedUser = new UserDTO { Id = userId };
            _mockUserService.Setup(x => x.GetById(userId)).Returns(expectedUser);

            // Act
            var result = _controller.GetById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(expectedUser.Id, returnedUser.Id);
        }

        [Fact]
        public void GetById_NonExistingUser_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserService.Setup(x => x.GetById(userId)).Returns((UserDTO)null);

            // Act
            var result = _controller.GetById(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetAll_ExistingUsers_ReturnsOkResult()
        {
            // Arrange
            var users = new List<UserDTO>
            {
                new UserDTO { Id = Guid.NewGuid() },
                new UserDTO { Id = Guid.NewGuid() }
            };
            _mockUserService.Setup(x => x.GetAll()).Returns(users);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsAssignableFrom<IEnumerable<UserDTO>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count());
        }

        [Fact]
        public void GetAll_NoUsers_ReturnsNotFound()
        {
            // Arrange
            _mockUserService.Setup(x => x.GetAll()).Returns(new List<UserDTO>());

            // Act
            var result = _controller.GetAll();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No users found.", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateRangeAsync_ValidRequests_ReturnsOkResult()
        {
            // Arrange
            var requests = new List<CreateUserRequest>
            {
                new CreateUserRequest { Username = "user1" },
                new CreateUserRequest { Username = "user2" }
            };
            var createdUsers = new List<UserDTO>
            {
                new UserDTO { Id = Guid.NewGuid() },
                new UserDTO { Id = Guid.NewGuid() }
            };
            _mockUserService.Setup(x => x.CreateRangeAsync(requests)).ReturnsAsync(createdUsers);

            // Act
            var result = await _controller.CreateRangeAsync(requests);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsAssignableFrom<IEnumerable<UserDTO>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count());
        }

        [Fact]
        public void DeleteRange_ExistingUsers_ReturnsOkResult()
        {
            // Arrange
            var userIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockUserService.Setup(x => x.DeleteRange(userIds)).Returns(true);

            // Act
            var result = _controller.DeleteRange(userIds);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Users deleted successfully.", okResult.Value);
        }

        [Fact]
        public void DeleteRange_NonExistingUsers_ReturnsNotFound()
        {
            // Arrange
            var userIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockUserService.Setup(x => x.DeleteRange(userIds)).Returns(false);

            // Act
            var result = _controller.DeleteRange(userIds);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No users found for the provided IDs.", notFoundResult.Value);
        }
    }
} 