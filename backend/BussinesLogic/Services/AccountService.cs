using BussinesLogic.EntityDtos;
using BussinesLogic.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace BussinesLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountService> _logger;
        private readonly IRepository<User> _userRepo;
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;

        public AccountService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AccountService> logger,
            IRepository<User> userRepository,
            IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _userRepo = userRepository;
            _userService = userService;
        }

        public async Task<bool> Register(RegisterDto request)
        {
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email
            };

            // Use UserManager to handle password hashing
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User registered successfully.");
                return true;
            }

            _logger.LogWarning("User registration failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
            return false;
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


            var token = GenerateJwtToken(user);
            _logger.LogInformation("User logged in successfully.");
            return token;
        }

        public async Task<bool> LogOut(Guid userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
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

