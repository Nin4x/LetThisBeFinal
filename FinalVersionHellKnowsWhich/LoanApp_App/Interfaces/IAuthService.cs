using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.UserDTOs;

namespace FinalVersionHellKnowsWhich.LoanApp_App.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(UserRegisterDTO dto);
        Task<string> LoginAsync(UserLoginDTO dto);
    }
}