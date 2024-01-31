using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApplication3.Model
{
    public class UserProfile : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string NRIC { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Column("Resume")]
        public byte[] Resume { get; set; }
        public string WhoAmI { get; set; }

        /*public List<string> PasswordHistory { get; set; } = new List<string>();

        public DateTime PasswordChangeTime { get; set; }

        public void AddPasswordToHistory(){
            PasswordHistory.Append(this.PasswordHash);
            if (PasswordHistory.Count > 5) {
                PasswordHistory.RemoveAt(0);
            }
            PasswordChangeTime = DateTime.Now;
        }
    }




    public class CustomPasswordValidator : IPasswordValidator<UserProfile>
    {
        private readonly int passwordHistorySize;

        public CustomPasswordValidator(int passwordHistorySize)
        {
            this.passwordHistorySize = passwordHistorySize;
        }

        public async Task<IdentityResult> ValidateAsync(UserManager<UserProfile> manager, UserProfile user, string password)
        {
            var passwordHistory = user.PasswordHistory;

            // Check against password history
            if (passwordHistory.Contains(password))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordReused",
                    Description = "Password has been used before and cannot be reused.",
                });
            }

            // Other password validation logic

            return IdentityResult.Success;
        }
    }

}*/
    }
}
