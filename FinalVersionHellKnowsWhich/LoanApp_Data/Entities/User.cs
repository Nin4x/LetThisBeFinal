using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace FinalVersionHellKnowsWhich.LoanApp_Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Username { get; set; }
        public string PasswordHash { get; set; } = null!;
        [EmailAddress]
        public required string Email { get; set; } 
        public int Age { get; set; }
        public decimal MonthlyIncome { get; set; }
        public bool IsBlocked { get; set; } = false;
        public Role UserRole { get; set; } = Role.User;
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();

    }

    public enum Role { User, Accountant }
}
