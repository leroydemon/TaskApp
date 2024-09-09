using BussinesLogic.EntityDtos;
using FluentValidation;

namespace Infrastructure.Validators
{
    public class TaskToDoDtoValidator : AbstractValidator<TaskToDoDto>
    {
        public TaskToDoDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty();

            RuleFor(x => x.Title)
                .NotEmpty();
        }
    }
}
