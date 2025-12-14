using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.UserDTOs;
using FinalVersionHellKnowsWhich.LoanApp_App.Security;
using FinalVersionHellKnowsWhich.LoanApp_Data.DB;
using FinalVersionHellKnowsWhich.LoanApp_Data.Entities;
using FinalVersionHellKnowsWhich.LoanApp_Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalVersionHellKnowsWhich.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly JwtTokenService _jwt;

        public AuthController(AppDbContext db, JwtTokenService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO dto)
        {
            var exists = await _db.Users.AnyAsync(u => u.Username == dto.Username || u.Email == dto.Email);
            if (exists) return BadRequest("Username or email already exists.");

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

            return Ok("Registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u =>
                u.Username == dto.UsernameOrEmail || u.Email == dto.UsernameOrEmail);

            if (user == null) return Unauthorized("Invalid credentials.");
            if (user.IsBlocked) return Forbid("User is blocked.");

            var ok = PasswordVerifier.Verify(dto.Password, user.PasswordHash);
            if (!ok) return Unauthorized("Invalid credentials.");

            var token = _jwt.CreateToken(user);

            return Ok(new { token });
        }
    }
}
