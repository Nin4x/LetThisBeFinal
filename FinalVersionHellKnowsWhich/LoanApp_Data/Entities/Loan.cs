using System.ComponentModel.DataAnnotations;
using FinalVersionHellKnowsWhich.LoanApp_Data.Enums;


namespace FinalVersionHellKnowsWhich.LoanApp_Data.Entities
{
    public class Loan
    {
        public Guid Id { get; set; }
        [Required]
        public LoanType Type { get; set; }
        [Required]
        public decimal Amount { get; set; } 
        [Required]
        public Currency Currency { get; set; }
        [Required]
        public int PeriodMonths { get; set; } 
        public LoanStatus Status { get; set; } = LoanStatus.Pending;
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }

   
}
