using System.ComponentModel.DataAnnotations;

namespace FinalVersionHellKnowsWhich.LoanApp_App.DTOs.UserDTOs
{
    public class UserLoginDTO
    {
        public required string UsernameOrEmail { get; set; }
        public required string Password { get; set; }
    }
}
