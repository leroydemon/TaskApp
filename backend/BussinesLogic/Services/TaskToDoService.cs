using AutoMapper;
using BussinesLogic.EntityDtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BussinesLogic.Services
{
    public class TaskToDoService
    {
        private readonly IRepository<TaskToDo> _repos;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TaskToDoService(IRepository<TaskToDo> repos, IMapper mapper, ILogger logger)
        {
            _repos = repos;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TaskToDoDto> FindByIdAsync(Guid id)
        {
            var user = await _repos.GetByIdAsync(id);

            return _mapper.Map<TaskToDoDto>(user);
        }

        public async Task<TaskToDoDto> AddAsync(TaskToDoDto taskDto)
        {
            var task = _mapper.Map<TaskToDo>(taskDto);

            return _mapper.Map<TaskToDoDto>(await _repos.AddAsync(task));
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await _repos.GetByIdAsync(id);
            await _repos.DeleteAsync(task);
        }

        public async Task<TaskToDoDto> UpdateAsync(TaskToDoDto taskDto)
        {
            var updatedTask = await _repos.UpdateAsync(_mapper.Map<TaskToDo>(taskDto));

            return _mapper.Map<TaskToDoDto>(updatedTask);
        }

        public async Task<IEnumerable<TaskToDoDto>> SearchAsync()
        {
            return new List<TaskToDoDto>();
        }
    }
}
