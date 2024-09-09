using BussinesLogic.EntityDtos;
using Domain.Entities;
using Domain.Filters;

namespace BussinesLogic.Interfaces
{
    public interface ITaskToDoService
    {
        Task<IEnumerable<TaskToDoDto>> SearchAsync(TaskToDoFilter filter);
        Task RemoveAsync(Guid taskId);
        Task<TaskToDoDto> GetByIdAsync(Guid taskId);
        Task<TaskToDoDto> UpdateAsync(TaskToDoDto user);
        Task<TaskToDoDto> AddAsync(TaskToDoDto user);
    }
}
