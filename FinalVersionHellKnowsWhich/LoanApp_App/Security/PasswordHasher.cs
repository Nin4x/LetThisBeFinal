using System.Text;
using System.Security.Cryptography;

namespace FinalVersionHellKnowsWhich.LoanApp_App.Security
{
    public class PasswordHasher
    {
        public static string Hash(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

    }
}
