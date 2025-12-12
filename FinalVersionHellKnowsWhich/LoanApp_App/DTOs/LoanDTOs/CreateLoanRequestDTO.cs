using FinalVersionHellKnowsWhich.LoanApp_Data.Entities;

namespace FinalVersionHellKnowsWhich.LoanApp_App.DTOs.LoanDTOs
{
    public class CreateLoanRequestDTO
    {   
        public Type Type { get; set; }
        public Currency Currency { get; set; }
        public decimal Amount { get; set; }
        public int PeriodMonths { get; set; }
    }
}
