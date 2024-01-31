
using Microsoft.AspNetCore.Identity;

namespace WebApplication3.Model
{


    public class YourHashingService
    {
        private readonly PasswordHasher<object> _passwordHasher;

        public YourHashingService()
        {
            _passwordHasher = new PasswordHasher<object>();
        }

        public string HashPassword(string password)
        {
            // The second parameter (null in this case) is the 'user' parameter, 
            // but since you're not associating it with any user, you can pass null.
            return _passwordHasher.HashPassword(null, password);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            // The second parameter (null in this case) is the 'user' parameter,
            // but since you're not associating it with any user, you can pass null.
            return _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
        }
    }

}
