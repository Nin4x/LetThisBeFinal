using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.LoanDTOs;
using FinalVersionHellKnowsWhich.LoanApp_App.Interfaces;
using FinalVersionHellKnowsWhich.LoanApp_Data.DB;
using FinalVersionHellKnowsWhich.LoanApp_Data.Entities;
using FinalVersionHellKnowsWhich.LoanApp_Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinalVersionHellKnowsWhich.LoanApp_App.Services
{
    public class UserLoanService : IUserLoansService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<UserLoanService> _logger;

        public UserLoanService(AppDbContext db, ILogger<UserLoanService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(Guid userId, UserCreateLoanRequestDTO dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) throw new KeyNotFoundException("User not found.");
            if (user.IsBlocked)
            {                
                _logger.LogWarning(
                        "Loan create attempt unsuccessful. Reason: User {userId} is blocked",
                        userId
                    );
                throw new UnauthorizedAccessException("User is blocked.");
            }

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

            _logger.LogInformation(

                "User {UserId} created {type} loan : ID: {LoanId}, Amount: {Amount} {Currency}, Date: {Date}",

                userId,
                loan.Type,
                loan.Id,
                loan.Amount,
                loan.Currency,
                DateTime.UtcNow
            );

            return loan.Id;
        }

        public async Task<List<UserLoanSearchResponseDTO>> GetMyLoansAsync(Guid userId)
        {
            return await _db.Loans
                .Where(x => x.UserId == userId)
                .Select(x => new UserLoanSearchResponseDTO
                {
                    Id = x.Id,
                    Type = x.Type,
                    Amount = x.Amount,
                    Currency = x.Currency,
                    PeriodMonths = x.PeriodMonths,
                    Status = x.Status
                })
                .ToListAsync();
        }

        public async Task UpdateAsync(Guid userId, Guid loanId, UpdateLoanRequestDTO dto)
        {
            var loan = await _db.Loans.FirstOrDefaultAsync(x => x.Id == loanId);
            if (loan == null) throw new KeyNotFoundException("Loan not found.");

            if (loan.UserId != userId) throw new UnauthorizedAccessException("Not your loan.");
            if (loan.Status != LoanStatus.Pending)
            {
                _logger.LogInformation(

                    "Loan edit attempt by User {UserId} failed. Reason : Loan ID: {LoanId}, Status is {Status}. Date: {Date} ",

                    userId,
                    loan.Id,
                    loan.Status,
                    DateTime.UtcNow
                );
                throw new InvalidOperationException("Only pending loans can be updated.");
            }

            loan.Type = dto.Type;
            loan.Amount = dto.Amount;
            loan.Currency = dto.Currency;
            loan.PeriodMonths = dto.PeriodMonths;

            await _db.SaveChangesAsync();

            _logger.LogInformation(

                "User {UserId} updated loan : ID: {LoanId}, Type: {type}, Amount: {Amount} {Currency}, Date: {Date}",

                userId,
                loan.Id,
                loan.Type,
                loan.Amount,
                loan.Currency,
                DateTime.UtcNow
            );
        }

        public async Task DeleteAsync(Guid userId, Guid loanId)
        {
            var loan = await _db.Loans.FirstOrDefaultAsync(x => x.Id == loanId);
            if (loan == null) throw new KeyNotFoundException("Loan not found.");

            if (loan.UserId != userId) throw new UnauthorizedAccessException("Not your loan.");
            if (loan.Status != LoanStatus.Pending)
            {
                _logger.LogInformation(

                    "Loan delete attempt by User {UserId} failed. Reason : Loan ID: {LoanId}, Status is {Status}.",

                    userId,
                    loan.Id,
                    loan.Status
                );
                throw new InvalidOperationException("Only pending loans can be deleted.");
            }
            _db.Loans.Remove(loan);
            await _db.SaveChangesAsync();
        }
    }
}
