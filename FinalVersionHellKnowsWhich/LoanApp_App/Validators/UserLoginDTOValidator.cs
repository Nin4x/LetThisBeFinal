using FluentValidation;
using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.UserDTOs;

namespace FinalVersionHellKnowsWhich.LoanApp_App.Validators
{
    public class UserLoginDTOValidator : AbstractValidator<UserLoginDTO>
    {
        public UserLoginDTOValidator()
        {
            RuleFor(x => x.UsernameOrEmail).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
