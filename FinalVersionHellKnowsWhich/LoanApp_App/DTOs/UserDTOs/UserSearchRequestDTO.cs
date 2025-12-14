namespace FinalVersionHellKnowsWhich.LoanApp_App.DTOs.UserDTOs
{
    public class UserSearchRequestDto
    {
        public Guid? Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
    }
}
