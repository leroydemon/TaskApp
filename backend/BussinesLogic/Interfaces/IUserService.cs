using BussinesLogic.EntityDtos;

namespace BussinesLogic.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> SearchAsync(List<UserDto> list);
        Task RemoveAsync(Guid userId);
        Task<UserDto> GetByIdAsync(Guid userId);
        Task<UserDto> UpdateAsync(UserDto user);
        Task<UserDto> AddAsync(UserDto user);
    }
}
