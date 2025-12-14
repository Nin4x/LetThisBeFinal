using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.UserDTOs;

namespace FinalVersionHellKnowsWhich.LoanApp_App.Interfaces
{
    public interface IAccountantUserService
    {
        Task<List<UserResponseDTO>> SearchAsync(UserSearchRequestDto dto);
        Task BlockAsync(Guid userId);
        Task UnblockAsync(Guid userId);
    }
}
