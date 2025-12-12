using System.ComponentModel.DataAnnotations;
using FinalVersionHellKnowsWhich.LoanApp_Data.Enums;


namespace FinalVersionHellKnowsWhich.LoanApp_Data.Entities
{
    public class Loan
    {
        public Guid Id { get; set; }
        public LoanType Type { get; set; }
        public decimal Amount { get; set; } 
        public Currency Currency { get; set; }
        public int PeriodMonths { get; set; } 
        public LoanStatus Status { get; set; } = LoanStatus.Pending;
        public Guid UserId { get; set; }
        public required User User { get; set; }
    }

   
}
