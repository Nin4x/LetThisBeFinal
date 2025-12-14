using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.UserDTOs;
using FinalVersionHellKnowsWhich.LoanApp_App.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalVersionHellKnowsWhich.LoanApp_App.Controllers
{
    [ApiController]
    [Route("api/accountant/users")]
    [Authorize(Roles = "Accountant")]
    public class AccountantUsersController : ControllerBase
    {
        private readonly IAccountantUserService _service;

        public AccountantUsersController(IAccountantUserService service)
        {
            _service = service;
        }

        [HttpPost("search")]
        public async Task<ActionResult<List<UserResponseDTO>>> Search([FromBody] UserSearchRequestDto dto)
        {
            try
            {
                var results = await _service.SearchAsync(dto);
                return Ok(results);
            }
            catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }

        [HttpPut("{id:guid}/block")]
        public async Task<IActionResult> BlockUser(Guid id)
        {
            try
            {
                await _service.BlockAsync(id);
                return Ok("Blocked.");
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }

        [HttpPut("{id:guid}/unblock")]
        public async Task<IActionResult> UnblockUser(Guid id)
        {
            try
            {
                await _service.UnblockAsync(id);
                return Ok("Unblocked.");
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }
    }
}
