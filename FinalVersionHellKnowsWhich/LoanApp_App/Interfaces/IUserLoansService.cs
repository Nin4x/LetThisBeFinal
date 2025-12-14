using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.LoanDTOs;

namespace FinalVersionHellKnowsWhich.LoanApp_App.Interfaces
{
    public interface IUserLoansService
    {
        Task<Guid> CreateAsync(Guid userId, UserCreateLoanRequestDTO dto);
        Task<List<UserLoanSearchResponseDTO>> GetMyLoansAsync(Guid userId);
        Task UpdateAsync(Guid userId, Guid loanId, UpdateLoanRequestDTO dto);
        Task DeleteAsync(Guid userId, Guid loanId);
    }
}
