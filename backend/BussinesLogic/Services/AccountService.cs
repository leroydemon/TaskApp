using BussinesLogic.EntityDtos;
using BussinesLogic.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Authorization;
using BussinesLogic.Results;
using Authorization.Interfaces;

namespace BussinesLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountService> _logger;
        private readonly IRepository<User> _repos;
        private readonly ITokenGeneratorService _tokenGenerator;
        private readonly SignInManager<User> _signInManager;

        public AccountService(
            ITokenGeneratorService tokenGenerator,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AccountService> logger,
            IRepository<User> userRepository,
            IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _repos = userRepository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<RegistrationResult> Register(RegisterDto request)
        {
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email
            };

            // Use UserManager to handle password hashing and user creation
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User registered successfully.");
                return new RegistrationResult
                {
                    Success = true,
                    Message = "User registered successfully."
                };
            }

            // If registration fails, gather all error descriptions
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogWarning("User registration failed: {Errors}", errors);

            return new RegistrationResult
            {
                Success = false,
                Message = $"User registration failed: {errors}"
            };
        }

        public async Task<string?> Login(LoginDto input)
        {
            var user = await _userManager.FindByNameAsync(input.UsernameOrEmail)
                        ?? await _userManager.FindByEmailAsync(input.UsernameOrEmail);

            if (user == null || !await _userManager.CheckPasswordAsync(user, input.Password))
            {
                _logger.LogWarning("Login failed: Invalid credentials.");

                return null;
            }

            var token = _tokenGenerator.GenerateJwtToken(user);
            _logger.LogInformation("User logged in successfully.");

            return token;
        }

        public async Task<bool> LogOut(Guid userId)
        {
            var user = await _repos.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Logout failed: User not found.");

                return false;
            }

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out successfully.");

            return true;
        }
    }
}

