using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.LoanDTOs;
using FinalVersionHellKnowsWhich.LoanApp_Data.Enums;
using FluentValidation;

namespace FinalVersionHellKnowsWhich.LoanApp_App.Validators
{
    public class UserCreateLoanRequestDTOValidator : AbstractValidator<UserCreateLoanRequestDTO>
    {
        public UserCreateLoanRequestDTOValidator()
        {

            RuleFor(x => x).Custom((dto, ctx) =>
            {
                var max = dto.Type switch
                {
                    LoanType.QuickLoan => 5000,
                    LoanType.Auto => 50000,
                    LoanType.Installment => 15000,
                    _ => 0
                };

                if (dto.Amount > max)
                    ctx.AddFailure("Amount", $"Max for {dto.Type} is {max}.");
            });
            RuleFor(x => x).Custom((dto, ctx) =>
            {
                var (min, max) = dto.Type switch
                {
                    LoanType.QuickLoan => (1, 12),
                    LoanType.Auto => (6, 84),
                    LoanType.Installment => (3, 60),
                    _ => (1, 360)
                };

                if (dto.PeriodMonths < min || dto.PeriodMonths > max)
                    ctx.AddFailure("PeriodMonths", $"{dto.Type} period must be between {min} and {max} months.");
            });
            RuleFor(x => x.Type).IsInEnum();
            RuleFor(x => x.Currency).IsInEnum();
        }

    }
}
