using AutoMapper;
using BussinesLogic.EntityDtos;
using BussinesLogic.Interfaces;
using Domain.Entities;
using Domain.Filters;
using Domain.Interfaces;
using Domain.Specification;
using Microsoft.Extensions.Logging;

namespace BussinesLogic.Services
{
    public class TaskToDoService : ITaskToDoService
    {
        private readonly IRepository<TaskToDo> _repos;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskToDoService> _logger;

        public TaskToDoService(IRepository<TaskToDo> repos, IMapper mapper, ILogger<TaskToDoService> logger)
        {
            _repos = repos;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TaskToDoDto> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Searching for task with ID {TaskId}", id);

            var task = await _repos.GetByIdAsync(id);

            _logger.LogInformation("Found task with ID {TaskId}", id);
            return _mapper.Map<TaskToDoDto>(task);
        }

        public async Task<TaskToDoDto> AddAsync(TaskToDoDto taskDto)
        {
            _logger.LogInformation("Adding a new task: {TaskTitle}", taskDto.Title);

            var task = _mapper.Map<TaskToDo>(taskDto);
            var addedTask = await _repos.AddAsync(task);

            _logger.LogInformation("Task with ID {TaskId} has been added", addedTask.Id);
            return _mapper.Map<TaskToDoDto>(addedTask);
        }

        public async Task RemoveAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete task with ID {TaskId}", id);

            var task = await _repos.GetByIdAsync(id);

            await _repos.DeleteAsync(task);
            _logger.LogInformation("Task with ID {TaskId} has been deleted", id);
        }

        public async Task<TaskToDoDto> UpdateAsync(TaskToDoDto taskDto)
        {
            _logger.LogInformation("Updating task with ID {TaskId}", taskDto.Id);

            var updatedTask = await _repos.UpdateAsync(_mapper.Map<TaskToDo>(taskDto));

            _logger.LogInformation("Task with ID {TaskId} has been updated", updatedTask.Id);
            return _mapper.Map<TaskToDoDto>(updatedTask);
        }

        public async Task<IEnumerable<TaskToDoDto>> SearchAsync(TaskToDoFilter filter)
        {
            _logger.LogInformation("Searching tasks with filter: {Filter}", filter);

            var spec = new TaskToDoSpecification(filter);
            var tasks = await _repos.ListAsync(spec);

            _logger.LogInformation("Found {TaskCount} tasks matching filter", tasks.Count());
            return _mapper.Map<List<TaskToDoDto>>(tasks);
        }
    }
}
