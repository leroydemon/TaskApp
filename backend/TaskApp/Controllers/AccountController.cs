using BussinesLogic.EntityDtos;
using BussinesLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TaskApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            _logger.LogInformation("Attempting to register a new user.");

            var result = await _accountService.Register(request);
            if (result)
            {
                _logger.LogInformation("User registered successfully.");
                return Ok(new { Message = "Registration successful" });
            }

            _logger.LogWarning("User registration failed.");
            return BadRequest(new { Message = "Registration failed" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto input)
        {
            _logger.LogInformation("Attempting to log in.");

            var token = await _accountService.Login(input);
            if (token != null)
            {
                _logger.LogInformation("User logged in successfully.");
                return Ok(new { Token = token });
            }

            _logger.LogWarning("Login failed. Invalid credentials.");
            return Unauthorized(new { Message = "Invalid credentials" });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut([FromQuery] Guid userId)  // Обычно лучше использовать [FromQuery] для ID
        {
            _logger.LogInformation("Attempting to log out user with ID {UserId}.", userId);

            var result = await _accountService.LogOut(userId);
            if (result)
            {
                _logger.LogInformation("User logged out successfully.");
                return NoContent();  // Используем NoContent для успешного логаута
            }

            _logger.LogWarning("Logout failed for user with ID {UserId}.", userId);
            return BadRequest(new { Message = "Logout failed" });
        }
    }

}
