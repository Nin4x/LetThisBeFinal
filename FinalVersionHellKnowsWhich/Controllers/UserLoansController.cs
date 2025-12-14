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
    public class UserLoansController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UserLoansController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost("NewLoan")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create([FromBody] CreateLoanRequestDTO dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) return Unauthorized();
            if (user.IsBlocked) return Forbid("User is blocked.");

            var loan = new Loan
            {
                Id = Guid.NewGuid(),
                UserId = userId,
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

        [HttpGet("MyLoans")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<List<UserLoanSearchResponseDTO>>> MyLoans()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var loans = await _db.Loans
                .Where(x => x.UserId == userId)
                .Select(x => new UserLoanSearchResponseDTO
                {
                    Id = x.Id,
                    Type = x.Type,
                    Amount = x.Amount,
                    Currency = x.Currency,
                    PeriodMonths = x.PeriodMonths,
                    Status =x.Status,
                })
                .ToListAsync();

            return Ok(loans);

        }
    }
}
