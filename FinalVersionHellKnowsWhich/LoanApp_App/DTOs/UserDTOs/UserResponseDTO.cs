namespace FinalVersionHellKnowsWhich.LoanApp_App.DTOs.UserDTOs
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public decimal MonthlyIncome { get; set; }
        public bool IsBlocked { get; set; }
        public string UserRole { get; set; }
    }
}
