using FinalVersionHellKnowsWhich.LoanApp_Data.Enums;

namespace FinalVersionHellKnowsWhich.LoanApp_App.DTOs.UserDTOs
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public int Age { get; set; }
        public decimal MonthlyIncome { get; set; }
        public bool IsBlocked { get; set; }
        public required UserRole.Role UserRole { get; set; }
    }
}
