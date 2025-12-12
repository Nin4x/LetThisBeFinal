using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace FinalVersionHellKnowsWhich.LoanApp_Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } 
        public int Age { get; set; }
        public decimal MonthlyIncome { get; set; }
        public bool IsBlocked { get; set; } = false;
        public Role UserRole { get; set; } = Role.User;
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();

    }

    public enum Role { User, Accountant }
}
