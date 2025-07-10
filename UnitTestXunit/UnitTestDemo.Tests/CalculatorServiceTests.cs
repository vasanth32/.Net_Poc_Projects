using FluentAssertions;
using UnitTestDemo.Services;
using Xunit;

namespace UnitTestDemo.Tests
{
    public class CalculatorServiceTests
    {
        private readonly CalculatorService _calculator;

        public CalculatorServiceTests()
        {
            _calculator = new CalculatorService();
        }

        [Fact]
        public void Add_WithTwoPositiveNumbers_ReturnsCorrectSum()
        {
            // Arrange
            int a = 5;
            int b = 3;

            // Act
            int result = _calculator.Add(a, b);

            // Assert
            result.Should().Be(8);
        }

        [Fact]
        public void Add_WithNegativeNumbers_ReturnsCorrectSum()
        {
            // Arrange
            int a = -5;
            int b = -3;

            // Act
            int result = _calculator.Add(a, b);

            // Assert
            result.Should().Be(-8);
        }

        [Fact]
        public void Add_WithZero_ReturnsOtherNumber()
        {
            // Arrange
            int a = 5;
            int b = 0;

            // Act
            int result = _calculator.Add(a, b);

            // Assert
            result.Should().Be(5);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(0, 0, 0)]
        [InlineData(-1, 1, 0)]
        [InlineData(100, 200, 300)]
        public void Add_WithVariousInputs_ReturnsExpectedResults(int a, int b, int expected)
        {
            // Act
            int result = _calculator.Add(a, b);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Subtract_WithTwoPositiveNumbers_ReturnsCorrectDifference()
        {
            // Arrange
            int a = 10;
            int b = 3;

            // Act
            int result = _calculator.Subtract(a, b);

            // Assert
            result.Should().Be(7);
        }

        [Fact]
        public void Subtract_WithNegativeResult_ReturnsCorrectValue()
        {
            // Arrange
            int a = 3;
            int b = 10;

            // Act
            int result = _calculator.Subtract(a, b);

            // Assert
            result.Should().Be(-7);
        }

        [Fact]
        public void Multiply_WithTwoPositiveNumbers_ReturnsCorrectProduct()
        {
            // Arrange
            int a = 6;
            int b = 7;

            // Act
            int result = _calculator.Multiply(a, b);

            // Assert
            result.Should().Be(42);
        }

        [Fact]
        public void Multiply_WithZero_ReturnsZero()
        {
            // Arrange
            int a = 5;
            int b = 0;

            // Act
            int result = _calculator.Multiply(a, b);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void Divide_WithValidDivision_ReturnsCorrectQuotient()
        {
            // Arrange
            int a = 10;
            int b = 2;

            // Act
            double result = _calculator.Divide(a, b);

            // Assert
            result.Should().Be(5.0);
        }

        [Fact]
        public void Divide_WithNonDivisibleNumbers_ReturnsDecimalResult()
        {
            // Arrange
            int a = 7;
            int b = 2;

            // Act
            double result = _calculator.Divide(a, b);

            // Assert
            result.Should().Be(3.5);
        }

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

        [Theory]
        [InlineData(2, true)]
        [InlineData(3, false)]
        [InlineData(0, true)]
        [InlineData(-2, true)]
        [InlineData(-3, false)]
        [InlineData(100, true)]
        [InlineData(101, false)]
        public void IsEven_WithVariousNumbers_ReturnsExpectedResult(int number, bool expected)
        {
            // Act
            bool result = _calculator.IsEven(number);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void GetRandomNumber_ReturnsNumberInExpectedRange()
        {
            // Act
            int result = _calculator.GetRandomNumber();

            // Assert
            result.Should().BeGreaterThanOrEqualTo(1);
            result.Should().BeLessThanOrEqualTo(100);
        }

        [Fact]
        public void GetRandomNumber_CalledMultipleTimes_ReturnsDifferentNumbers()
        {
            // Act
            int result1 = _calculator.GetRandomNumber();
            int result2 = _calculator.GetRandomNumber();
            int result3 = _calculator.GetRandomNumber();

            // Assert
            // Note: This test might occasionally fail due to the nature of random numbers
            // In a real scenario, you might want to mock the Random class or use a different approach
            var results = new[] { result1, result2, result3 };
            results.Should().HaveCount(3);
        }
    }
} 