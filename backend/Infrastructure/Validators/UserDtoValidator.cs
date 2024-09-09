using BussinesLogic.EntityDtos;
using FluentValidation;

namespace Infrastructure.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
        }
    }
}
