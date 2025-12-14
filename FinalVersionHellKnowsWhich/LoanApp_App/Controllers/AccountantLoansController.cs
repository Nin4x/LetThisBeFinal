using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.LoanDTOs;
using FinalVersionHellKnowsWhich.LoanApp_App.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalVersionHellKnowsWhich.LoanApp_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Accountant")]
    public class AccountantLoansController : ControllerBase
    {
        private readonly IAccountantLoanService _service;

        public AccountantLoansController(IAccountantLoanService service)
        {
            _service = service;
        }

        [HttpPost("NewLoan")]
        public async Task<IActionResult> Create([FromBody] AccountantCreateLoanRequestDTO dto)
        {
            try
            {
                var loanId = await _service.CreateAsync(dto);
                return Ok(new { Id = loanId });
            }
            catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }

        // ✅ fixed route binding
        [HttpGet("{userId:guid}/UserLoans")]
        public async Task<ActionResult<List<AccountantLoanSearchResponseDTO>>> UserLoans(Guid userId)
        {
            var loans = await _service.GetUserLoansAsync(userId);
            return Ok(loans);
        }

        [HttpPut("{id:guid}/LoanUpdate")]
        public async Task<IActionResult> UpdateLoan(Guid id, [FromBody] UpdateLoanRequestDTO dto)
        {
            try
            {
                await _service.UpdateLoanAsync(id, dto);
                return Ok("Loan Updated.");
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }

        [HttpPut("{id:guid}/LoanStatusUpdate")]
        public async Task<IActionResult> UpdateLoanStatus(Guid id, [FromBody] UpdateLoanRequestStatusDTO dto)
        {
            try
            {
                await _service.UpdateStatusAsync(id, dto);
                return Ok("Status Updated.");
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }

        [HttpDelete("{id:guid}/DeleteLoan")]
        public async Task<IActionResult> DeleteLoan(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok("Loan Deleted.");
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }
    }
}
