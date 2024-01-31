using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.ViewModels;
using WebApplication3.Model;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Encodings.Web;

using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.ReCaptcha;


namespace WebApplication3.Pages
{
    [ValidateReCaptcha]
    public class RegisterModel : PageModel
    {
        private const string RecaptchaSecretKey = "6Le68mApAAAAAE_Ew9UoI7K07kR5Miv7yvHRu4FP";


        private UserManager<UserProfile> userManager { get; }
        private SignInManager<UserProfile> signInManager { get; }

        [BindProperty]
        public Register RModel { get; set; }

        public RegisterModel(UserManager<UserProfile> userManager,
        SignInManager<UserProfile> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }



        public void OnGet()
        {
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
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> OnPostAsync()
        {
            var recaptchaResponse = Request.Form["g-recaptcha-response"];
            var isRecaptchaValid = await ValidateRecaptcha(recaptchaResponse);

            if (!isRecaptchaValid)
            {
                ModelState.AddModelError("", "reCAPTCHA validation failed. Please try again.");
                return Page();
            }

            byte[] ResumeFileContent = Array.Empty<byte>();
            if (RModel.Resume != null)
            {
                // ... (existing code for file validation)

                if (!IsPasswordComplex(RModel.Password))
                {
                    ModelState.AddModelError("RModel.Password", "Password must meet complexity requirements.");
                    return Page();
                }

                var existingUser = await userManager.Users.FirstOrDefaultAsync(u => u.Email == RModel.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("RModel.Email", "Email address is already in use. Please choose a different email.");
                    return Page();
                }


                var Encrypter = DataProtectionProvider.Create("EncryptData");
                var ProtectData = Encrypter.CreateProtector("MySecretKey");
                var encoder = UrlEncoder.Create();


                var user = new UserProfile()
                {
                    UserName = WebUtility.HtmlEncode(RModel.FirstName + RModel.LastName),
                    FirstName = WebUtility.HtmlEncode(RModel.FirstName),
                    LastName = WebUtility.HtmlEncode(RModel.LastName),
                    Gender = WebUtility.HtmlEncode(RModel.Gender),
                    NRIC = ProtectData.Protect(WebUtility.HtmlEncode(RModel.NRIC)),
                    Email = WebUtility.HtmlEncode(RModel.Email),
                    PasswordHash = WebUtility.HtmlEncode(RModel.Password),
                    DateOfBirth = RModel.DateOfBirth,
                    Resume = ResumeFileContent,
                    WhoAmI = WebUtility.HtmlEncode(RModel.WhoAmI),
                };


                var result = await userManager.CreateAsync(user, RModel.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToPage("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return Page();
        }
        private async Task<bool> ValidateRecaptcha(string recaptchaResponse)
        {
            using (var httpClient = new HttpClient())
            {
                var content = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("secret", RecaptchaSecretKey),
            new KeyValuePair<string, string>("response", recaptchaResponse)
        };

                var response = await httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(content));

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response and check if it's successful
                    // Example: {"success": true, "score": 0.9, "action": "submit_form"}
                    // You may customize this based on your reCAPTCHA configuration
                    // For v3, the response should have a "success" property with value true
                    return responseBody.Contains("\"success\": true");
                }

                return false;
            }
        }

    }

}