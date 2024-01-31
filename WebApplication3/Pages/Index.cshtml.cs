using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using WebApplication3.Model;
using System.Net;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Encodings.Web;
using WebApplication3.ViewModels;
using System.Text.RegularExpressions;

namespace WebApplication3.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DecryptedNRIC { get; set; }
        public string WhoAmI {  get; set; }
        public string password { get; set; }

        private UserManager<UserProfile> UserManager { get; }

        [BindProperty]
        public Register IModel { get; set; }

        public IndexModel(UserManager<UserProfile> userManager)
        {
            this.UserManager = userManager;
        }





        public async Task<IActionResult> OnGet()
        {
            var user = await UserManager.GetUserAsync(User);

            if (user != null)
            {
                var Encrypter = DataProtectionProvider.Create("EncryptData");
                var ProtectData = Encrypter.CreateProtector("MySecretKey");
                var encoder = UrlEncoder.Create();
                // Decrypt NRIC before displaying
                DecryptedNRIC = ProtectData.Unprotect(user.NRIC);

                FirstName = user.FirstName;
                LastName = user.LastName;
                Gender = user.Gender;
                DateOfBirth = user.DateOfBirth;
                Email = user.Email;
                WhoAmI = user.WhoAmI;
                password = user.PasswordHash;




                return Page();
            }

            // Handle the case where the user is not found
            return RedirectToPage("/Login"); // You can redirect to an error page or handle it as needed
        }

        private bool IsPasswordComplex(string password)
        {
            // Add your password complexity requirements here
            var hasLowerCase = Regex.IsMatch(password, @"[a-z]");
            var hasUpperCase = Regex.IsMatch(password, @"[A-Z]");
            var hasNumber = Regex.IsMatch(password, @"\d");
            var hasSpecialCharacter = Regex.IsMatch(password, @"[!@#\$%\^&\*\(\)_\+{}\[\]:;<>,.?~\\/\-]");

            return hasLowerCase && hasUpperCase && hasNumber && hasSpecialCharacter;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var recaptchaResponse = Request.Form["g-recaptcha-response"];

           

                if (!IsPasswordComplex(IModel.Password))
                {
                    ModelState.AddModelError("RModel.Password", "Password must meet complexity requirements.");
                    return Page();
                }
            var user = await UserManager.GetUserAsync(User);
            var result = await UserManager.ChangePasswordAsync(user, "OldPassword", "NewPassword");



            return Page();
        }
    }
}

