using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.LoanDTOs;
using FinalVersionHellKnowsWhich.LoanApp_Data.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalVersionHellKnowsWhich.LoanApp_App.DTOs.LoanDTOs
{
    public class AccountantUsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AccountantUsersController(AppDbContext db)
        {
            _db = db;
        }
   
        [HttpPut("{id:guid}/block")]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> BlockUser(Guid id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.IsBlocked = true;
            await _db.SaveChangesAsync();
            return Ok("Blocked.");
        }

        [HttpPut("{id:guid}/unblock")]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> UnblockUser(Guid id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.IsBlocked = false;
            await _db.SaveChangesAsync();
            return Ok("Unblocked.");
        }
    }
}
