using AutoMapper;
using BussinesLogic.EntityDtos;
using Domain.Entities;
using Domain.Filters;
using Domain.Interfaces;
using Domain.Specialization;
using Microsoft.Extensions.Logging;

namespace BussinesLogic.Services
{
    public class UserService
    {
        private readonly IRepository<User> _repos;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IRepository<User> repos, IMapper mapper, ILogger<UserService> logger)
        {
            _repos = repos;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching user with ID {UserId}", id);

            var user = await _repos.GetByIdAsync(id);

            _logger.LogInformation("User with ID {UserId} found", id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> AddAsync(UserDto userDto)
        {
            _logger.LogInformation("Adding new user");

            var user = _mapper.Map<User>(userDto);
            var addedUser = await _repos.AddAsync(user);

            _logger.LogInformation("User with ID {UserId} added", addedUser.Id);
            return _mapper.Map<UserDto>(addedUser);
        }

        public async Task DeleteAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete user with ID {UserId}", id);

            var user = await _repos.GetByIdAsync(id);

            await _repos.DeleteAsync(user);
            _logger.LogInformation("User with ID {UserId} deleted", id);
        }

        public async Task<UserDto> UpdateAsync(UserDto userDto)
        {
            _logger.LogInformation("Updating user with ID {UserId}", userDto.Id);

            var updatedUser = await _repos.UpdateAsync(_mapper.Map<User>(userDto));

            _logger.LogInformation("User with ID {UserId} updated", updatedUser.Id);
            return _mapper.Map<UserDto>(updatedUser);
        }

        public async Task<IEnumerable<UserDto>> SearchAsync(UserFilter filter)
        {
            _logger.LogInformation("Searching users with filter: {Filter}", filter);

            var spec = new UserSpecification(filter);
            var users = await _repos.ListAsync(spec);

            _logger.LogInformation("{UserCount} users found", users.Count());
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}
