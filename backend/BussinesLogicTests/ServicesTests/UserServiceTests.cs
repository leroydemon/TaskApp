using AutoMapper;
using BussinesLogic.EntityDtos;
using BussinesLogic.Services;
using Domain.Entities;
using Domain.Filters;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BussinesLogicTests.ServicesTests
{
    public class UserServiceTests
    {
        private readonly Mock<IRepository<User>> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _repositoryMock = new Mock<IRepository<User>>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _userService = new UserService(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_UserExists_ReturnsUserDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, UserName = "John Doe" };
            var userDto = new UserDto { Id = userId, UserName = "John Doe" };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(userDto);
        }

        [Fact]
        public async Task RemoveAsync_UserExists_RemovesUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            await _userService.RemoveAsync(userId);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ValidUserDto_ReturnsUpdatedUserDto()
        {
            // Arrange
            var userDto = new UserDto { Id = Guid.NewGuid(), UserName = "Updated Name" };
            var user = new User { Id = userDto.Id, UserName = "Updated Name" };
            var updatedUser = new User { Id = userDto.Id, UserName = "Updated Name" };

            _mapperMock.Setup(m => m.Map<User>(userDto)).Returns(user);
            _repositoryMock.Setup(repo => repo.UpdateAsync(user)).ReturnsAsync(updatedUser);
            _mapperMock.Setup(m => m.Map<UserDto>(updatedUser)).Returns(userDto);

            // Act
            var result = await _userService.UpdateAsync(userDto);

            // Assert
            result.Should().BeEquivalentTo(userDto);
        }

        [Fact]
        public async Task SearchAsync_ValidFilter_ReturnsMatchingUsers()
        {
            // Arrange
            var filter = new UserFilter { UserName = "John" };
            var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), UserName = "John Doe" },
            new User { Id = Guid.NewGuid(), UserName = "John Smith" }
        };
            var userDtos = users.Select(u => new UserDto { Id = u.Id, UserName = u.UserName }).ToList();

            _repositoryMock.Setup(repo => repo.ListAsync(It.IsAny<ISpecification<User>>())).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<List<UserDto>>(users)).Returns(userDtos);

            // Act
            var result = await _userService.SearchAsync(filter);

            // Assert
            result.Should().BeEquivalentTo(userDtos);
        }
    }
}
