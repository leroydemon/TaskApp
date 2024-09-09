using BussinesLogic.EntityDtos;
using BussinesLogic.Results;

namespace BussinesLogic.Interfaces
{
    public interface IAccountService
    {
        Task<RegistrationResult> Register(RegisterDto request);
        Task<string> Login(LoginDto input);
        Task<bool> LogOut(Guid userId);
    }
}
