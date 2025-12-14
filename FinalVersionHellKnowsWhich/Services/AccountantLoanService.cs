using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.LoanDTOs;
using FinalVersionHellKnowsWhich.LoanApp_App.Interfaces;
using FinalVersionHellKnowsWhich.LoanApp_Data.DB;
using FinalVersionHellKnowsWhich.LoanApp_Data.Entities;
using FinalVersionHellKnowsWhich.LoanApp_Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalVersionHellKnowsWhich.LoanApp_App.Services
{
    public class AccountantLoanService : IAccountantLoanService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<AccountantLoanService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountantLoanService(AppDbContext db, ILogger<AccountantLoanService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Guid> CreateAsync(AccountantCreateLoanRequestDTO dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == dto.UserId);
            if (user == null) throw new KeyNotFoundException("User not found.");
            if (user.IsBlocked) throw new UnauthorizedAccessException("User is blocked.");

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
            return loan.Id;
        }

        public async Task<List<AccountantLoanSearchResponseDTO>> GetUserLoansAsync(Guid userId)
        {
            return await _db.Loans
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
        }

        public async Task UpdateLoanAsync(Guid loanId, UpdateLoanRequestDTO dto)
        {
            var loan = await _db.Loans.FirstOrDefaultAsync(x => x.Id == loanId);
            if (loan == null) throw new KeyNotFoundException("Loan not found.");

            loan.Type = dto.Type;
            loan.Amount = dto.Amount;
            loan.Currency = dto.Currency;
            loan.PeriodMonths = dto.PeriodMonths;

            await _db.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(Guid loanId, UpdateLoanRequestStatusDTO dto)
        {
            var loan = await _db.Loans.FirstOrDefaultAsync(x => x.Id == loanId);
            if (loan == null) throw new KeyNotFoundException("Loan not found.");

            loan.Status = dto.Status;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid loanId)
        {
            var loan = await _db.Loans.FirstOrDefaultAsync(x => x.Id == loanId);
            if (loan == null) throw new KeyNotFoundException("Loan not found.");

            _db.Loans.Remove(loan);
            await _db.SaveChangesAsync();
        }
    }
}
