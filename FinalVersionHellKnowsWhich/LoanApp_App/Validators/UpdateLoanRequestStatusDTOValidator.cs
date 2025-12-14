using FluentValidation;
using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.LoanDTOs;

namespace FinalVersionHellKnowsWhich.LoanApp_App.Validators
{
    public class UpdateLoanRequestStatusDTOValidator : AbstractValidator<UpdateLoanRequestStatusDTO>
    {
        public UpdateLoanRequestStatusDTOValidator()
        {
            RuleFor(x => x.Status).IsInEnum();
        }
    }
}
