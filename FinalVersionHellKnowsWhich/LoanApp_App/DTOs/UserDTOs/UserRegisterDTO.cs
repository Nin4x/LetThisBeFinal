using System.ComponentModel.DataAnnotations;
using FinalVersionHellKnowsWhich.LoanApp_Data.Enums;

namespace FinalVersionHellKnowsWhich.LoanApp_App.DTOs.UserDTOs
{
    public class UserRegisterDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public int Age { get; set; }
        public decimal MonthlyIncome { get; set; }
        public required string Password { get; set; }
    }
}
