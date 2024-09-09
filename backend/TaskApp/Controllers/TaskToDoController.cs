using BussinesLogic.EntityDtos;
using BussinesLogic.Interfaces;
using Domain.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskToDoController : ControllerBase
    {
        private readonly ITaskToDoService _taskService;

        public TaskToDoController(ITaskToDoService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("search")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<ActionResult<IEnumerable<UserDto>>> SearchAsync([FromQuery] TaskToDoFilter filter)
        {
            var users = await _taskService.SearchAsync(filter);

            return Ok(users);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> AddAsync([FromBody] TaskToDoDto user)
        {
            await _taskService.AddAsync(user);

            return Ok(user);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> UpdateAsync([FromBody] TaskToDoDto user)
        {
            await _taskService.UpdateAsync(user);

            return Ok();
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> DeleteAsync(Guid userId)
        {
            await _taskService.RemoveAsync(userId);

            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var user = await _taskService.GetByIdAsync(id);

            return Ok(user);
        }
    }
}
