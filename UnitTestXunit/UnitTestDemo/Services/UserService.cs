using System.ComponentModel.DataAnnotations;
using UnitTestDemo.Models;

namespace UnitTestDemo.Services
{
    public class UserService : IUserService
    {
        public User CreateUser(string name, string email, int age)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            if (age < 18 || age > 120)
                throw new ArgumentException("Age must be between 18 and 120", nameof(age));

            return new User
            {
                Name = name.Trim(),
                Email = email.Trim().ToLower(),
                Age = age,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        public bool ValidateUser(User user)
        {
            if (user == null)
                return false;

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(user);

            return Validator.TryValidateObject(user, validationContext, validationResults, true);
        }

        public List<User> GetActiveUsers(List<User> users)
        {
            if (users == null)
                return new List<User>();

            return users.Where(u => u.IsActive).ToList();
        }

        public User? GetUserById(List<User> users, int id)
        {
            if (users == null)
                return null;

            return users.FirstOrDefault(u => u.Id == id);
        }

        public bool IsEmailUnique(List<User> users, string email)
        {
            if (users == null || string.IsNullOrWhiteSpace(email))
                return false;

            return !users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
} 