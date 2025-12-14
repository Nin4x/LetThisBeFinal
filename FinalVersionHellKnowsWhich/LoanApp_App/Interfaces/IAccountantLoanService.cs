using FinalVersionHellKnowsWhich.LoanApp_App.DTOs.LoanDTOs;

namespace FinalVersionHellKnowsWhich.LoanApp_App.Interfaces
{
    public interface IAccountantLoanService
    {
        Task<Guid> CreateAsync(AccountantCreateLoanRequestDTO dto);
        Task<List<AccountantLoanSearchResponseDTO>> GetUserLoansAsync(Guid userId);
        Task UpdateLoanAsync(Guid loanId, UpdateLoanRequestDTO dto);
        Task UpdateStatusAsync(Guid loanId, UpdateLoanRequestStatusDTO dto);
        Task DeleteAsync(Guid loanId);
    }
}
