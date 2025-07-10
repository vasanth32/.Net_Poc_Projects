namespace UnitTestDemo.Services
{
    public class CalculatorService : ICalculatorService
    {
        private readonly Random _random;

        public CalculatorService()
        {
            _random = new Random();
        }

        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Subtract(int a, int b)
        {
            return a - b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }

        public double Divide(int a, int b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero");
            }
            return (double)a / b;
        }

        public bool IsEven(int number)
        {
            return number % 2 == 0;
        }

        public int GetRandomNumber()
        {
            return _random.Next(1, 101); // Returns a number between 1 and 100
        }
    }
} 