using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Model;
using WebApplication3.ViewModels;
using System.Text.Json;
using AspNetCore.ReCaptcha;


namespace WebApplication3.Pages
{
    [ValidateReCaptcha]
    public class LoginModel : PageModel
    {
        
        [BindProperty]
        public Login LModel { get; set; }

        private readonly UserManager<UserProfile> userManager;
        private readonly SignInManager<UserProfile> signInManager;

        public LoginModel(UserManager<UserProfile> userManager, SignInManager<UserProfile> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public void OnGet()
        {
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {


            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Index");
            }

            var user = await userManager.FindByEmailAsync(LModel.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, LModel.Password))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return Page();
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, LModel.Password, LModel.RememberMe, false);

            if (signInResult.Succeeded)
            {
                return RedirectToPage("/Index");
            }
            else if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Account locked out due to multiple failed login attempts. Please try again later.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
            }

            return Page();
        }
    }
}
