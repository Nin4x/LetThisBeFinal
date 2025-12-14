using FluentValidation;
using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.UserDTOs;

namespace FinalVersionHellKnowsWhich.LoanApp_App.Validators
{
    public class UserRegisterDTOValidator : AbstractValidator<UserRegisterDTO>
    {
        public UserRegisterDTOValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2).MaximumLength(32);
            RuleFor(x => x.LastName).NotEmpty().MinimumLength(2).MaximumLength(32);
            RuleFor(x => x.Username).NotEmpty().MinimumLength(3).MaximumLength(32);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(50);

            RuleFor(x => x.Age).InclusiveBetween(18, 120);
            RuleFor(x => x.MonthlyIncome).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Password)
              .NotEmpty()
              .MinimumLength(8)
              .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter.")
              .Matches("[a-z]").WithMessage("Password must contain a lowercase letter.")
              .Matches("[0-9]").WithMessage("Password must contain a number.")
              .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain a special character.");
        }
    }
}
