using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.UserDTOs;
using FinalVersionHellKnowsWhich.LoanApp_App.Interfaces;
using FinalVersionHellKnowsWhich.LoanApp_Data.DB;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalVersionHellKnowsWhich.LoanApp_App.Services
{
    public class AccountantUserService : IAccountantUserService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<AccountantUserService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AccountantUserService(AppDbContext db, ILogger<AccountantUserService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        private Guid GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?
                .User?
                .FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                throw new UnauthorizedAccessException("User not authenticated.");

            return Guid.Parse(userId);
        }
        public async Task<List<UserResponseDTO>> SearchAsync(UserSearchRequestDto dto)
        {
            if (dto.Id.HasValue)
            {
                var user = await _db.Users
                    .Where(u => u.Id == dto.Id.Value)
                    .Select(u => new UserResponseDTO
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Username = u.Username,
                        Email = u.Email,
                        Age = u.Age,
                        MonthlyIncome = u.MonthlyIncome,
                        IsBlocked = u.IsBlocked,
                        UserRole = u.UserRole
                    })
                    .FirstOrDefaultAsync();

                if (user == null) throw new KeyNotFoundException("User not found.");
                return new List<UserResponseDTO> { user };
            }

            if (string.IsNullOrWhiteSpace(dto.FirstName) ||
                string.IsNullOrWhiteSpace(dto.LastName) ||
                dto.Age == null)
            {
                throw new InvalidOperationException("Provide either Id, or FirstName + LastName + Age.");
            }

            var firstName = dto.FirstName.Trim();
            var lastName = dto.LastName.Trim();
            var age = dto.Age.Value;

            var results = await _db.Users
                .Where(u => u.FirstName == firstName && u.LastName == lastName && u.Age == age)
                .Select(u => new UserResponseDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Username = u.Username,
                    Email = u.Email,
                    Age = u.Age,
                    MonthlyIncome = u.MonthlyIncome,
                    IsBlocked = u.IsBlocked,
                    UserRole = u.UserRole
                })
                .ToListAsync();

            if (results.Count == 0) throw new KeyNotFoundException("No users matched your search.");
            return results;
        }

        public async Task BlockAsync(Guid userId)
        {
            var accountantId = GetCurrentUserId();
            var user = await _db.Users.FindAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found.");

            user.IsBlocked = true;
            await _db.SaveChangesAsync();
            _logger.LogInformation(

                    "{userId} has been blocked by {accountantId} {date} ",

                    userId,
                    accountantId,
                    DateTime.UtcNow
                );
        }

        public async Task UnblockAsync(Guid userId)
        {
            var accountantId = GetCurrentUserId();
            var user = await _db.Users.FindAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found.");

            user.IsBlocked = false;
            await _db.SaveChangesAsync();
            _logger.LogInformation(

                    "{userId} has been unblocked by {accountantId} ",

                    userId,
                    accountantId
                );
        }
    }
}