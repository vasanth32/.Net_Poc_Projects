using FluentAssertions;
using UnitTestDemo.Models;
using UnitTestDemo.Services;
using Xunit;

namespace UnitTestDemo.Tests
{
    public class UserServiceTests
    {
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userService = new UserService();
        }

        [Fact]
        public void CreateUser_WithValidData_ReturnsValidUser()
        {
            // Arrange
            string name = "John Doe";
            string email = "john@example.com";
            int age = 25;

            // Act
            var user = _userService.CreateUser(name, email, age);

            // Assert
            user.Should().NotBeNull();
            user.Name.Should().Be("John Doe");
            user.Email.Should().Be("john@example.com");
            user.Age.Should().Be(25);
            user.IsActive.Should().BeTrue();
            user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void CreateUser_WithWhitespaceInName_TrimsName()
        {
            // Arrange
            string name = "  John Doe  ";
            string email = "john@example.com";
            int age = 25;

            // Act
            var user = _userService.CreateUser(name, email, age);

            // Assert
            user.Name.Should().Be("John Doe");
        }

        [Fact]
        public void CreateUser_WithMixedCaseEmail_ConvertsToLowerCase()
        {
            // Arrange
            string name = "John Doe";
            string email = "JOHN@EXAMPLE.COM";
            int age = 25;

            // Act
            var user = _userService.CreateUser(name, email, age);

            // Assert
            user.Email.Should().Be("john@example.com");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void CreateUser_WithEmptyName_ThrowsArgumentException(string name)
        {
            // Arrange
            string email = "john@example.com";
            int age = 25;

            // Act & Assert
            var action = () => _userService.CreateUser(name, email, age);
            action.Should().Throw<ArgumentException>()
                  .WithMessage("Name cannot be empty*")
                  .And.ParamName.Should().Be("name");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void CreateUser_WithEmptyEmail_ThrowsArgumentException(string email)
        {
            // Arrange
            string name = "John Doe";
            int age = 25;

            // Act & Assert
            var action = () => _userService.CreateUser(name, email, age);
            action.Should().Throw<ArgumentException>()
                  .WithMessage("Email cannot be empty*")
                  .And.ParamName.Should().Be("email");
        }

        [Theory]
        [InlineData(17)]
        [InlineData(121)]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateUser_WithInvalidAge_ThrowsArgumentException(int age)
        {
            // Arrange
            string name = "John Doe";
            string email = "john@example.com";

            // Act & Assert
            var action = () => _userService.CreateUser(name, email, age);
            action.Should().Throw<ArgumentException>()
                  .WithMessage("Age must be between 18 and 120*")
                  .And.ParamName.Should().Be("age");
        }

        [Theory]
        [InlineData(18)]
        [InlineData(25)]
        [InlineData(120)]
        public void CreateUser_WithValidAge_DoesNotThrowException(int age)
        {
            // Arrange
            string name = "John Doe";
            string email = "john@example.com";

            // Act & Assert
            var action = () => _userService.CreateUser(name, email, age);
            action.Should().NotThrow();
        }

        [Fact]
        public void ValidateUser_WithValidUser_ReturnsTrue()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@example.com",
                Age = 25,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            bool isValid = _userService.ValidateUser(user);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void ValidateUser_WithNullUser_ReturnsFalse()
        {
            // Act
            bool isValid = _userService.ValidateUser(null);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void ValidateUser_WithInvalidEmail_ReturnsFalse()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Name = "John Doe",
                Email = "invalid-email",
                Age = 25,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            bool isValid = _userService.ValidateUser(user);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void ValidateUser_WithInvalidAge_ReturnsFalse()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@example.com",
                Age = 17, // Invalid age
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            bool isValid = _userService.ValidateUser(user);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void GetActiveUsers_WithMixedUsers_ReturnsOnlyActiveUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "John", Email = "john@example.com", Age = 25, IsActive = true },
                new User { Id = 2, Name = "Jane", Email = "jane@example.com", Age = 30, IsActive = false },
                new User { Id = 3, Name = "Bob", Email = "bob@example.com", Age = 35, IsActive = true }
            };

            // Act
            var activeUsers = _userService.GetActiveUsers(users);

            // Assert
            activeUsers.Should().HaveCount(2);
            activeUsers.Should().OnlyContain(u => u.IsActive);
            activeUsers.Should().Contain(u => u.Name == "John");
            activeUsers.Should().Contain(u => u.Name == "Bob");
        }

        [Fact]
        public void GetActiveUsers_WithNullList_ReturnsEmptyList()
        {
            // Act
            var activeUsers = _userService.GetActiveUsers(null);

            // Assert
            activeUsers.Should().BeEmpty();
        }

        [Fact]
        public void GetActiveUsers_WithEmptyList_ReturnsEmptyList()
        {
            // Arrange
            var users = new List<User>();

            // Act
            var activeUsers = _userService.GetActiveUsers(users);

            // Assert
            activeUsers.Should().BeEmpty();
        }

        [Fact]
        public void GetUserById_WithExistingUser_ReturnsUser()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "John", Email = "john@example.com", Age = 25 },
                new User { Id = 2, Name = "Jane", Email = "jane@example.com", Age = 30 }
            };

            // Act
            var user = _userService.GetUserById(users, 1);

            // Assert
            user.Should().NotBeNull();
            user!.Name.Should().Be("John");
        }

        [Fact]
        public void GetUserById_WithNonExistingUser_ReturnsNull()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "John", Email = "john@example.com", Age = 25 }
            };

            // Act
            var user = _userService.GetUserById(users, 999);

            // Assert
            user.Should().BeNull();
        }

        [Fact]
        public void GetUserById_WithNullList_ReturnsNull()
        {
            // Act
            var user = _userService.GetUserById(null, 1);

            // Assert
            user.Should().BeNull();
        }

        [Fact]
        public void IsEmailUnique_WithUniqueEmail_ReturnsTrue()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "John", Email = "john@example.com", Age = 25 }
            };
            string newEmail = "jane@example.com";

            // Act
            bool isUnique = _userService.IsEmailUnique(users, newEmail);

            // Assert
            isUnique.Should().BeTrue();
        }

        [Fact]
        public void IsEmailUnique_WithExistingEmail_ReturnsFalse()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "John", Email = "john@example.com", Age = 25 }
            };
            string existingEmail = "john@example.com";

            // Act
            bool isUnique = _userService.IsEmailUnique(users, existingEmail);

            // Assert
            isUnique.Should().BeFalse();
        }

        [Fact]
        public void IsEmailUnique_WithCaseInsensitiveMatch_ReturnsFalse()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "John", Email = "john@example.com", Age = 25 }
            };
            string existingEmail = "JOHN@EXAMPLE.COM";

            // Act
            bool isUnique = _userService.IsEmailUnique(users, existingEmail);

            // Assert
            isUnique.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void IsEmailUnique_WithEmptyEmail_ReturnsFalse(string email)
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "John", Email = "john@example.com", Age = 25 }
            };

            // Act
            bool isUnique = _userService.IsEmailUnique(users, email);

            // Assert
            isUnique.Should().BeFalse();
        }

        [Fact]
        public void IsEmailUnique_WithNullList_ReturnsFalse()
        {
            // Arrange
            string email = "john@example.com";

            // Act
            bool isUnique = _userService.IsEmailUnique(null, email);

            // Assert
            isUnique.Should().BeFalse();
        }
    }
} 