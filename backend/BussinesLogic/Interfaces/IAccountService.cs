using BussinesLogic.EntityDtos;

namespace BussinesLogic.Interfaces
{
    public interface IAccountService
    {
        Task<bool> Register(RegisterDto request);
        Task<string> Login(LoginDto input);
        Task<bool> LogOut(Guid userId);
    }
}
