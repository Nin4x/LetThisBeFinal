using FluentValidation;
using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.UserDTOs;

namespace FinalVersionHellKnowsWhich.LoanApp_App.Validators
{
    public class UserSearchRequestDtoValidator : AbstractValidator<UserSearchRequestDto>
    {
        public UserSearchRequestDtoValidator()
        {
            RuleFor(x => x).Must(ValidSearch)
                .WithMessage("Provide either Id OR FirstName + LastName + Age.");
        }

        private bool ValidSearch(UserSearchRequestDto dto)
        {
            if (dto.Id.HasValue) return true;

            return !string.IsNullOrWhiteSpace(dto.FirstName)
                && !string.IsNullOrWhiteSpace(dto.LastName)
                && dto.Age.HasValue;
        }
    }
}
