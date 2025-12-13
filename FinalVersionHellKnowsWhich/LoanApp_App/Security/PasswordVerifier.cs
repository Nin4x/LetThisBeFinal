namespace FinalVersionHellKnowsWhich.LoanApp_App.Security;

public class PasswordVerifier
    {
        public static bool Verify(string password, string storedHash)
        {
            var hash = PasswordHasher.Hash(password);
            return hash == storedHash;
        }
    }

