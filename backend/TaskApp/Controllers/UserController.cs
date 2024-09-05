using BussinesLogic.EntityDtos;
using BussinesLogic.Interfaces;
using Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TaskApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<UserDto>>> SearchAsync([FromQuery] UserFilter filter)
        {
            var users = await _userService.SearchAsync(filter);

            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] UserDto user)
        {
            await _userService.AddAsync(user);

            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UserDto user)
        {
            await _userService.UpdateAsync(user);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(Guid userId)
        {
            await _userService.RemoveAsync(userId);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);

            return Ok(user);
        }
    }
}
