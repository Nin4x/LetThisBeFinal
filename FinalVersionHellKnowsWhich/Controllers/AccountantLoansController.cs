using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.LoanDTOs;
using FinalVersionHellKnowsWhich.LoanApp_Data.DB;
using FinalVersionHellKnowsWhich.LoanApp_Data.Entities;
using FinalVersionHellKnowsWhich.LoanApp_Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalVersionHellKnowsWhich.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountantLoansController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AccountantLoansController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost("{id:guid}/NewLoan")]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> Create([FromBody] AccountantCreateLoanRequestDTO dto)
        {

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == dto.UserId);
            if (user == null) return Unauthorized();
            if (user.IsBlocked) return Forbid("User is blocked.");

            var loan = new Loan
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                Type = dto.Type,
                Amount = dto.Amount,
                Currency = dto.Currency,
                PeriodMonths = dto.PeriodMonths,
                Status = LoanStatus.Pending,
                User = user
            };

            _db.Loans.Add(loan);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                loan.Id
            });
        }

        [HttpGet("{id:guid}/UserLoans")]
        [Authorize(Roles = "Accountant")]
        public async Task<ActionResult<List<AccountantLoanSearchResponseDTO>>> UserLoans(Guid UserId)
        {
            var userId = UserId;

            var loans = await _db.Loans
                .Where(x => x.UserId == userId)
                .Select(x => new AccountantLoanSearchResponseDTO
                {
                    Id = x.Id,
                    Type = x.Type,
                    Amount = x.Amount,
                    Currency = x.Currency,
                    PeriodMonths = x.PeriodMonths,
                    Status = x.Status,
                    UserId = x.UserId
                })
                .ToListAsync();

            return Ok(loans);

        }

        [HttpPut("{id:guid}/LoanUpdate")]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> UpdateLoan(Guid id, UpdateLoanRequestDTO dto)
        {

            var loan = await _db.Loans.FirstOrDefaultAsync(x => x.Id == id);
            if (loan == null) return NotFound("Loan not found.");

            loan.Type = dto.Type;
            loan.Amount = dto.Amount;
            loan.Currency = dto.Currency;
            loan.PeriodMonths = dto.PeriodMonths;

            await _db.SaveChangesAsync();
            return Ok("Loan Updated.");
        }

        [HttpPut("{id:guid}/LoanStatusUpdate")]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> UpdateLoanStatus(Guid id, UpdateLoanRequestStatusDTO dto)
        {

            var loan = await _db.Loans.FirstOrDefaultAsync(x => x.Id == id);
            if (loan == null) return NotFound("Loan not found.");

            loan.Status = dto.Status;

            await _db.SaveChangesAsync();
            return Ok("Status Updated.");
        }


        [HttpDelete("{id:guid}/DeleteLoan")]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> DeleteLoan(Guid id)
        {
            var loan = await _db.Loans.FirstOrDefaultAsync(x => x.Id == id);
            if (loan == null) return NotFound("Loan not found.");

            _db.Loans.Remove(loan);
            await _db.SaveChangesAsync();

            return Ok("Loan Deleted.");
        }

    }
}
