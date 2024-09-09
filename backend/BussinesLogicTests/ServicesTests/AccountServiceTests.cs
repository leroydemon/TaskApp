using Authorization;
using Authorization.Interfaces;
using BussinesLogic.EntityDtos;
using BussinesLogic.Interfaces;
using BussinesLogic.Services;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BussinesLogicTests.ServicesTests
{
    public class AccountServiceTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<ILogger<AccountService>> _loggerMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<ITokenGeneratorService> _tokenGeneratorMock;
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _loggerMock = new Mock<ILogger<AccountService>>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _tokenGeneratorMock = new Mock<ITokenGeneratorService>();
            _signInManagerMock = new Mock<SignInManager<User>>(
                _userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                null, null, null, null);

            _accountService = new AccountService(
                _tokenGeneratorMock.Object,
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _loggerMock.Object,
                _userRepositoryMock.Object,
                Mock.Of<IUserService>());
        }

        [Fact]
        public async Task Register_UserCreatedSuccessfully_ReturnsSuccess()
        {
            // Arrange
            var request = new RegisterDto { UserName = "testuser", Email = "test@example.com", Password = "Password123!" };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), request.Password))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _accountService.Register(request);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("User registered successfully.");
        }

        [Fact]
        public async Task Register_UserCreationFailed_ReturnsFailure()
        {
            // Arrange
            var request = new RegisterDto { UserName = "testuser", Email = "test@example.com", Password = "Password123!" };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), request.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password too weak" }));

            // Act
            var result = await _accountService.Register(request);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Password too weak");
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginDto = new LoginDto { UsernameOrEmail = "testuser", Password = "Password123!" };
            var user = new User { UserName = "testuser", Email = "test@example.com" };

            _userManagerMock.Setup(um => um.FindByNameAsync(loginDto.UsernameOrEmail)).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.CheckPasswordAsync(user, loginDto.Password)).ReturnsAsync(true);
            _tokenGeneratorMock.Setup(tg => tg.GenerateJwtToken(user)).Returns("valid_token");

            // Act
            var token = await _accountService.Login(loginDto);

            // Assert
            token.Should().Be("valid_token");
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            var loginDto = new LoginDto { UsernameOrEmail = "testuser", Password = "WrongPassword!" };
            _userManagerMock.Setup(um => um.FindByNameAsync(loginDto.UsernameOrEmail)).ReturnsAsync((User)null);

            // Act
            var token = await _accountService.Login(loginDto);

            // Assert
            token.Should().BeNull();
        }

        [Fact]
        public async Task LogOut_ValidUser_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _accountService.LogOut(userId);

            // Assert
            result.Should().BeTrue();
            _signInManagerMock.Verify(sm => sm.SignOutAsync(), Times.Once);
        }

        [Fact]
        public async Task LogOut_UserNotFound_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((User?)null); 

            // Act
            var result = await _accountService.LogOut(userId);

            // Assert
            result.Should().BeFalse(); 
        }

    }
}
