using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.UserDTOs;
using FinalVersionHellKnowsWhich.LoanApp_App.Interfaces;
using FinalVersionHellKnowsWhich.LoanApp_App.Security;
using FinalVersionHellKnowsWhich.LoanApp_Data.DB;
using FinalVersionHellKnowsWhich.LoanApp_Data.Entities;
using FinalVersionHellKnowsWhich.LoanApp_Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinalVersionHellKnowsWhich.LoanApp_App.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly JwtTokenService _jwt;
        private readonly ILogger<AuthService> _logger;

        public AuthService(AppDbContext db, JwtTokenService jwt, ILogger<AuthService> logger)
        {
            _db = db;
            _jwt = jwt;
            _logger = logger;
        }

        public async Task RegisterAsync(UserRegisterDTO dto)
        {
            var exists = await _db.Users.AnyAsync(u => u.Username == dto.Username || u.Email == dto.Email);
            if (exists) throw new InvalidOperationException("Username or email already exists.");

            var hash = PasswordHasher.Hash(dto.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Email = dto.Email,
                Age = dto.Age,
                MonthlyIncome = dto.MonthlyIncome,
                PasswordHash = hash,
                UserRole = UserRole.Role.User,
                IsBlocked = false
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            _logger.LogInformation("New user registered: {Username} ({Email})", 
                user.Username,
                user.Email
                );
        }

        public async Task<string> LoginAsync(UserLoginDTO dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u =>
                u.Username == dto.UsernameOrEmail || u.Email == dto.UsernameOrEmail);

            if (user == null)
            {
                _logger.LogWarning("Failed login attempt for {Login}.", 
                    dto.UsernameOrEmail
                    );
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            if (user.IsBlocked)
            {
                _logger.LogWarning("Blocked user {UserId} attempted login.",
                    user.Id
                    );
                throw new UnauthorizedAccessException("User is blocked.");
            }

            var ok = PasswordVerifier.Verify(dto.Password, user.PasswordHash);
            if (!ok)
            {
                _logger.LogWarning("Failed login attempt for {Login}.",
                    dto.UsernameOrEmail
                    );
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            _logger.LogInformation("User logged in: {UserId} ({Username}).}",
                user.Id,
                user.Username
                );
            return _jwt.CreateToken(user);
        }
    }
}
