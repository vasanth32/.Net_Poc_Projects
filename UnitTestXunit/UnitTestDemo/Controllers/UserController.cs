using Microsoft.AspNetCore.Mvc;
using UnitTestDemo.Models;
using UnitTestDemo.Services;

namespace UnitTestDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private static List<User> _users = new List<User>();

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                var user = _userService.CreateUser(request.Name, request.Email, request.Age);
                
                if (!_userService.IsEmailUnique(_users, user.Email))
                {
                    return BadRequest(new { error = "Email already exists" });
                }

                user.Id = _users.Count + 1;
                _users.Add(user);

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _userService.GetUserById(_users, id);
            
            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetUsers([FromQuery] bool activeOnly = false)
        {
            var users = activeOnly ? _userService.GetActiveUsers(_users) : _users;
            return Ok(users);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            var user = _userService.GetUserById(_users, id);
            
            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            if (!string.IsNullOrEmpty(request.Name))
                user.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Email))
            {
                if (!_userService.IsEmailUnique(_users.Where(u => u.Id != id).ToList(), request.Email))
                {
                    return BadRequest(new { error = "Email already exists" });
                }
                user.Email = request.Email;
            }

            if (request.Age.HasValue)
                user.Age = request.Age.Value;

            if (!_userService.ValidateUser(user))
            {
                return BadRequest(new { error = "Invalid user data" });
            }

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _userService.GetUserById(_users, id);
            
            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            user.IsActive = false;
            return Ok(new { message = "User deactivated successfully" });
        }
    }

    public class CreateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    public class UpdateUserRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? Age { get; set; }
    }
} 