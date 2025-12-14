using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.LoanDTOs;
using FinalVersionHellKnowsWhich.LoanApp_App.Interfaces;
using FinalVersionHellKnowsWhich.LoanApp_App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalVersionHellKnowsWhich.LoanApp_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserLoansController : ControllerBase
    {
        private readonly IUserLoansService _service;

        public UserLoansController(IUserLoansService service)
        {
            _service = service;
        }

        [HttpPost("NewLoan")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create([FromBody] UserCreateLoanRequestDTO dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            try
            {
                var loanId = await _service.CreateAsync(userId, dto);
                return Ok(new { Id = loanId });
            }
            catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }

        [HttpGet("MyLoans")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<List<UserLoanSearchResponseDTO>>> MyLoans()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var loans = await _service.GetMyLoansAsync(userId);
            return Ok(loans);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateMyLoan(Guid id, [FromBody] UpdateLoanRequestDTO dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            try
            {
                await _service.UpdateAsync(userId, id, dto);
                return Ok("Updated.");
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
            catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteMyLoan(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            try
            {
                await _service.DeleteAsync(userId, id);
                return Ok("Deleted.");
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
            catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        }
    }
}