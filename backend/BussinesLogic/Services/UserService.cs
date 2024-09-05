using AutoMapper;
using BussinesLogic.EntityDtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BussinesLogic.Services
{
    public class UserService
    {
        private readonly IRepository<User> _repos;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UserService(IRepository<User> repos, IMapper mapper, ILogger logger)
        {
            _repos = repos;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var user = await _repos.GetByIdAsync(id);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> AddAsync(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            return _mapper.Map<UserDto>(await _repos.AddAsync(user));
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _repos.GetByIdAsync(id);
            await _repos.DeleteAsync(user);
        }

        public async Task<UserDto> UpdateAsync(UserDto userDto)
        {
            var updatedUser = await _repos.UpdateAsync(_mapper.Map<User>(userDto));

            return _mapper.Map<UserDto>(updatedUser);
        }

        public async Task<IEnumerable<User>> SearchAsync()
        {
            return new List<User>(); //TO DO
        }
    }
}
