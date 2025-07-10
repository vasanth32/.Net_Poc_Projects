using UnitTestDemo.Models;

namespace UnitTestDemo.Services
{
    public interface IUserService
    {
        User CreateUser(string name, string email, int age);
        bool ValidateUser(User user);
        List<User> GetActiveUsers(List<User> users);
        User? GetUserById(List<User> users, int id);
        bool IsEmailUnique(List<User> users, string email);
    }
} 