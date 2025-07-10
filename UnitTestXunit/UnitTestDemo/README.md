# Unit Testing with xUnit - Learning Project

This project demonstrates comprehensive unit testing concepts using xUnit, Moq, and FluentAssertions in a .NET Core Web API.

## Project Structure

```
UnitTestDemo/
├── Controllers/           # API Controllers
│   ├── CalculatorController.cs
│   └── UserController.cs
├── Models/               # Data Models
│   └── User.cs
├── Services/            # Business Logic Services
│   ├── ICalculatorService.cs
│   ├── CalculatorService.cs
│   ├── IUserService.cs
│   └── UserService.cs
└── UnitTestDemo.Tests/  # Unit Tests
    ├── CalculatorServiceTests.cs
    ├── UserServiceTests.cs
    └── ControllerTests.cs
```

## Unit Testing Concepts Demonstrated

### 1. Basic Testing Structure (AAA Pattern)

Every test follows the **Arrange-Act-Assert** pattern:

```csharp
[Fact]
public void Add_WithTwoPositiveNumbers_ReturnsCorrectSum()
{
    // Arrange - Set up test data and dependencies
    int a = 5;
    int b = 3;

    // Act - Execute the method being tested
    int result = _calculator.Add(a, b);

    // Assert - Verify the expected outcome
    result.Should().Be(8);
}
```

### 2. Test Attributes

- **`[Fact]`** - For tests that don't require parameters
- **`[Theory]`** - For parameterized tests with `[InlineData]`

```csharp
[Theory]
[InlineData(1, 2, 3)]
[InlineData(0, 0, 0)]
[InlineData(-1, 1, 0)]
public void Add_WithVariousInputs_ReturnsExpectedResults(int a, int b, int expected)
{
    int result = _calculator.Add(a, b);
    result.Should().Be(expected);
}
```

### 3. Exception Testing

Testing that methods throw expected exceptions:

```csharp
[Fact]
public void Divide_ByZero_ThrowsDivideByZeroException()
{
    // Arrange
    int a = 10;
    int b = 0;

    // Act & Assert
    var action = () => _calculator.Divide(a, b);
    action.Should().Throw<DivideByZeroException>()
          .WithMessage("Cannot divide by zero");
}
```

### 4. Dependency Injection and Mocking

Using Moq to create mock dependencies:

```csharp
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
        _mockCalculatorService.Setup(x => x.Add(5, 3)).Returns(8);

        // Act
        var result = _controller.Add(5, 3);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }
}
```

### 5. FluentAssertions

Using FluentAssertions for more readable assertions:

```csharp
// Basic assertions
result.Should().Be(8);
result.Should().NotBeNull();
result.Should().BeGreaterThan(0);

// Collection assertions
users.Should().HaveCount(2);
users.Should().OnlyContain(u => u.IsActive);

// Exception assertions
action.Should().Throw<ArgumentException>()
      .WithMessage("Name cannot be empty*");

// Type assertions
result.Should().BeOfType<OkObjectResult>();
```

### 6. Testing Edge Cases

Testing boundary conditions and edge cases:

```csharp
[Theory]
[InlineData(17)]  // Below minimum age
[InlineData(121)] // Above maximum age
[InlineData(0)]   // Zero age
[InlineData(-1)]  // Negative age
public void CreateUser_WithInvalidAge_ThrowsArgumentException(int age)
{
    var action = () => _userService.CreateUser("John", "john@example.com", age);
    action.Should().Throw<ArgumentException>();
}
```

### 7. Testing Null and Empty Values

```csharp
[Theory]
[InlineData("")]
[InlineData("   ")]
[InlineData(null)]
public void CreateUser_WithEmptyName_ThrowsArgumentException(string name)
{
    var action = () => _userService.CreateUser(name, "john@example.com", 25);
    action.Should().Throw<ArgumentException>();
}
```

### 8. Testing Collections and LINQ Operations

```csharp
[Fact]
public void GetActiveUsers_WithMixedUsers_ReturnsOnlyActiveUsers()
{
    // Arrange
    var users = new List<User>
    {
        new User { Name = "John", IsActive = true },
        new User { Name = "Jane", IsActive = false },
        new User { Name = "Bob", IsActive = true }
    };

    // Act
    var activeUsers = _userService.GetActiveUsers(users);

    // Assert
    activeUsers.Should().HaveCount(2);
    activeUsers.Should().OnlyContain(u => u.IsActive);
}
```

### 9. Testing Controllers

Testing API controllers with HTTP response types:

```csharp
[Fact]
public void GetUser_WithExistingUser_ReturnsOkResult()
{
    // Arrange
    var user = new User { Id = 1, Name = "John" };
    _mockUserService.Setup(x => x.GetUserById(It.IsAny<List<User>>(), 1))
        .Returns(user);

    // Act
    var result = _controller.GetUser(1);

    // Assert
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result as OkObjectResult;
    okResult!.Value.Should().BeEquivalentTo(user);
}
```

### 10. Testing Validation

Testing data validation logic:

```csharp
[Fact]
public void ValidateUser_WithInvalidEmail_ReturnsFalse()
{
    // Arrange
    var user = new User
    {
        Name = "John Doe",
        Email = "invalid-email", // Invalid email format
        Age = 25
    };

    // Act
    bool isValid = _userService.ValidateUser(user);

    // Assert
    isValid.Should().BeFalse();
}
```

## Running the Tests

### Build and Run Tests
```bash
# Navigate to the project directory
cd UnitTestDemo

# Build the solution
dotnet build

# Run all tests
dotnet test

# Run tests with verbose output
dotnet test --verbosity normal

# Run tests for a specific project
dotnet test UnitTestDemo.Tests/

# Run tests with coverage (requires coverlet.collector package)
dotnet test --collect:"XPlat Code Coverage"
```

### Running Specific Tests
```bash
# Run tests with a specific name pattern
dotnet test --filter "FullyQualifiedName~CalculatorServiceTests"

# Run tests with a specific trait
dotnet test --filter "Category=Integration"
```

## Key Testing Principles Demonstrated

1. **Single Responsibility** - Each test focuses on one specific behavior
2. **Independence** - Tests don't depend on each other
3. **Repeatability** - Tests produce the same results every time
4. **Fast Execution** - Tests run quickly
5. **Clear Naming** - Test names describe the scenario and expected outcome
6. **Comprehensive Coverage** - Tests cover happy path, edge cases, and error conditions

## Best Practices Shown

1. **Test Naming Convention**: `MethodName_Scenario_ExpectedResult`
2. **Arrange-Act-Assert Pattern**: Clear separation of test phases
3. **Mock External Dependencies**: Isolate the unit under test
4. **Test One Thing**: Each test verifies one specific behavior
5. **Use Meaningful Test Data**: Realistic and descriptive test data
6. **Test Both Success and Failure Cases**: Verify error handling
7. **Use Parameterized Tests**: Reduce code duplication for similar scenarios

## API Endpoints

The project includes a working API with the following endpoints:

### Calculator API
- `GET /api/calculator/add?a={a}&b={b}` - Add two numbers
- `GET /api/calculator/subtract?a={a}&b={b}` - Subtract two numbers
- `GET /api/calculator/multiply?a={a}&b={b}` - Multiply two numbers
- `GET /api/calculator/divide?a={a}&b={b}` - Divide two numbers
- `GET /api/calculator/is-even/{number}` - Check if number is even
- `GET /api/calculator/random` - Get a random number

### User API
- `POST /api/user` - Create a new user
- `GET /api/user/{id}` - Get user by ID
- `GET /api/user?activeOnly={bool}` - Get all users or only active users
- `PUT /api/user/{id}` - Update user
- `DELETE /api/user/{id}` - Deactivate user

## Next Steps

1. **Add Integration Tests** - Test the full API with real HTTP requests
2. **Add Test Categories** - Use `[Trait]` attributes to categorize tests
3. **Add Test Data Builders** - Create helper classes for test data
4. **Add Test Fixtures** - Share setup code between tests
5. **Add Performance Tests** - Test performance characteristics
6. **Add Code Coverage** - Measure test coverage percentage

This project serves as a comprehensive example of unit testing best practices in .NET Core applications. 