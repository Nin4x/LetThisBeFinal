using FinalVersionHellKnowsWhich.LoanApp_Data.Entities;
using FinalVersionHellKnowsWhich.LoanApp_Data.Enums;

namespace FinalVersionHellKnowsWhich.LoanApp_App.DTOs.LoanDTOs
{
    public class LoanSearchResponseDTO
    {
        public Guid Id { get; set; }
        public LoanType Type { get; set; }
        public Currency Currency { get; set; }
        public decimal Amount { get; set; }
        public int PeriodMonths { get; set; }
        public LoanStatus Status { get; set; }
        public Guid UserId { get; set; }
    }
}
