using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UnitTestDemo.Controllers;
using UnitTestDemo.Services;
using Xunit;

namespace UnitTestDemo.Tests
{
    public class CalculatorControllerTests
    {
        private readonly Mock<ICalculatorService> _mockCalculatorService;
        private readonly CalculatorController _controller;

        public CalculatorControllerTests()
        {
            _mockCalculatorService = new Mock<ICalculatorService>();
            _controller = new CalculatorController(_mockCalculatorService.Object);
        }

        [Fact]
        public void Add_WithValidInput_ReturnsOkResult()
        {
            // Arrange
            int a = 5;
            int b = 3;
            int expectedResult = 8;
            _mockCalculatorService.Setup(x => x.Add(a, b)).Returns(expectedResult);

            // Act
            var result = _controller.Add(a, b);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().NotBeNull();
            
            // You could also test the anonymous object properties if needed
            var response = okResult.Value;
            response.Should().NotBeNull();
        }

        [Fact]
        public void Add_WhenServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            int a = 5;
            int b = 3;
            _mockCalculatorService.Setup(x => x.Add(a, b))
                .Throws(new Exception("Service error"));

            // Act
            var result = _controller.Add(a, b);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult!.Value.Should().NotBeNull();
        }

        [Fact]
        public void Subtract_WithValidInput_ReturnsOkResult()
        {
            // Arrange
            int a = 10;
            int b = 3;
            int expectedResult = 7;
            _mockCalculatorService.Setup(x => x.Subtract(a, b)).Returns(expectedResult);

            // Act
            var result = _controller.Subtract(a, b);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void Multiply_WithValidInput_ReturnsOkResult()
        {
            // Arrange
            int a = 6;
            int b = 7;
            int expectedResult = 42;
            _mockCalculatorService.Setup(x => x.Multiply(a, b)).Returns(expectedResult);

            // Act
            var result = _controller.Multiply(a, b);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void Divide_WithValidInput_ReturnsOkResult()
        {
            // Arrange
            int a = 10;
            int b = 2;
            double expectedResult = 5.0;
            _mockCalculatorService.Setup(x => x.Divide(a, b)).Returns(expectedResult);

            // Act
            var result = _controller.Divide(a, b);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void Divide_ByZero_ReturnsBadRequest()
        {
            // Arrange
            int a = 10;
            int b = 0;
            _mockCalculatorService.Setup(x => x.Divide(a, b))
                .Throws(new DivideByZeroException("Cannot divide by zero"));

            // Act
            var result = _controller.Divide(a, b);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult!.Value.Should().NotBeNull();
        }

        [Fact]
        public void IsEven_WithValidInput_ReturnsOkResult()
        {
            // Arrange
            int number = 4;
            bool expectedResult = true;
            _mockCalculatorService.Setup(x => x.IsEven(number)).Returns(expectedResult);

            // Act
            var result = _controller.IsEven(number);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetRandomNumber_ReturnsOkResult()
        {
            // Arrange
            int expectedResult = 42;
            _mockCalculatorService.Setup(x => x.GetRandomNumber()).Returns(expectedResult);

            // Act
            var result = _controller.GetRandomNumber();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetRandomNumber_WhenServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            _mockCalculatorService.Setup(x => x.GetRandomNumber())
                .Throws(new Exception("Random number generation failed"));

            // Act
            var result = _controller.GetRandomNumber();

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }

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
        public void CreateUser_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            var request = new UserController.CreateUserRequest
            {
                Name = "John Doe",
                Email = "john@example.com",
                Age = 25
            };

            var createdUser = new UnitTestDemo.Models.User
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@example.com",
                Age = 25,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _mockUserService.Setup(x => x.CreateUser(request.Name, request.Email, request.Age))
                .Returns(createdUser);
            _mockUserService.Setup(x => x.IsEmailUnique(It.IsAny<List<UnitTestDemo.Models.User>>(), request.Email))
                .Returns(true);

            // Act
            var result = _controller.CreateUser(request);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result as CreatedAtActionResult;
            createdResult!.Value.Should().BeEquivalentTo(createdUser);
        }

        [Fact]
        public void CreateUser_WithDuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            var request = new UserController.CreateUserRequest
            {
                Name = "John Doe",
                Email = "john@example.com",
                Age = 25
            };

            var createdUser = new UnitTestDemo.Models.User
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@example.com",
                Age = 25,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _mockUserService.Setup(x => x.CreateUser(request.Name, request.Email, request.Age))
                .Returns(createdUser);
            _mockUserService.Setup(x => x.IsEmailUnique(It.IsAny<List<UnitTestDemo.Models.User>>(), request.Email))
                .Returns(false);

            // Act
            var result = _controller.CreateUser(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult!.Value.Should().NotBeNull();
        }

        [Fact]
        public void CreateUser_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var request = new UserController.CreateUserRequest
            {
                Name = "",
                Email = "john@example.com",
                Age = 25
            };

            _mockUserService.Setup(x => x.CreateUser(request.Name, request.Email, request.Age))
                .Throws(new ArgumentException("Name cannot be empty"));

            // Act
            var result = _controller.CreateUser(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void GetUser_WithExistingUser_ReturnsOkResult()
        {
            // Arrange
            int userId = 1;
            var user = new UnitTestDemo.Models.User
            {
                Id = userId,
                Name = "John Doe",
                Email = "john@example.com",
                Age = 25
            };

            _mockUserService.Setup(x => x.GetUserById(It.IsAny<List<UnitTestDemo.Models.User>>(), userId))
                .Returns(user);

            // Act
            var result = _controller.GetUser(userId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(user);
        }

        [Fact]
        public void GetUser_WithNonExistingUser_ReturnsNotFound()
        {
            // Arrange
            int userId = 999;
            _mockUserService.Setup(x => x.GetUserById(It.IsAny<List<UnitTestDemo.Models.User>>(), userId))
                .Returns((UnitTestDemo.Models.User?)null);

            // Act
            var result = _controller.GetUser(userId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public void GetUsers_WithActiveOnlyFalse_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<UnitTestDemo.Models.User>
            {
                new UnitTestDemo.Models.User { Id = 1, Name = "John", Email = "john@example.com", Age = 25, IsActive = true },
                new UnitTestDemo.Models.User { Id = 2, Name = "Jane", Email = "jane@example.com", Age = 30, IsActive = false }
            };

            // Act
            var result = _controller.GetUsers(activeOnly: false);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetUsers_WithActiveOnlyTrue_ReturnsOnlyActiveUsers()
        {
            // Arrange
            var allUsers = new List<UnitTestDemo.Models.User>
            {
                new UnitTestDemo.Models.User { Id = 1, Name = "John", Email = "john@example.com", Age = 25, IsActive = true },
                new UnitTestDemo.Models.User { Id = 2, Name = "Jane", Email = "jane@example.com", Age = 30, IsActive = false }
            };

            var activeUsers = new List<UnitTestDemo.Models.User>
            {
                new UnitTestDemo.Models.User { Id = 1, Name = "John", Email = "john@example.com", Age = 25, IsActive = true }
            };

            _mockUserService.Setup(x => x.GetActiveUsers(It.IsAny<List<UnitTestDemo.Models.User>>()))
                .Returns(activeUsers);

            // Act
            var result = _controller.GetUsers(activeOnly: true);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void UpdateUser_WithValidData_ReturnsOkResult()
        {
            // Arrange
            int userId = 1;
            var request = new UserController.UpdateUserRequest
            {
                Name = "John Updated",
                Age = 26
            };

            var existingUser = new UnitTestDemo.Models.User
            {
                Id = userId,
                Name = "John Doe",
                Email = "john@example.com",
                Age = 25,
                IsActive = true
            };

            _mockUserService.Setup(x => x.GetUserById(It.IsAny<List<UnitTestDemo.Models.User>>(), userId))
                .Returns(existingUser);
            _mockUserService.Setup(x => x.ValidateUser(It.IsAny<UnitTestDemo.Models.User>()))
                .Returns(true);

            // Act
            var result = _controller.UpdateUser(userId, request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void UpdateUser_WithNonExistingUser_ReturnsNotFound()
        {
            // Arrange
            int userId = 999;
            var request = new UserController.UpdateUserRequest
            {
                Name = "John Updated"
            };

            _mockUserService.Setup(x => x.GetUserById(It.IsAny<List<UnitTestDemo.Models.User>>(), userId))
                .Returns((UnitTestDemo.Models.User?)null);

            // Act
            var result = _controller.UpdateUser(userId, request);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public void DeleteUser_WithExistingUser_ReturnsOkResult()
        {
            // Arrange
            int userId = 1;
            var user = new UnitTestDemo.Models.User
            {
                Id = userId,
                Name = "John Doe",
                Email = "john@example.com",
                Age = 25,
                IsActive = true
            };

            _mockUserService.Setup(x => x.GetUserById(It.IsAny<List<UnitTestDemo.Models.User>>(), userId))
                .Returns(user);

            // Act
            var result = _controller.DeleteUser(userId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void DeleteUser_WithNonExistingUser_ReturnsNotFound()
        {
            // Arrange
            int userId = 999;
            _mockUserService.Setup(x => x.GetUserById(It.IsAny<List<UnitTestDemo.Models.User>>(), userId))
                .Returns((UnitTestDemo.Models.User?)null);

            // Act
            var result = _controller.DeleteUser(userId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
} 