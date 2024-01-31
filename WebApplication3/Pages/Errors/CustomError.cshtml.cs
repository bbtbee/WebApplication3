using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication3.Pages.Errors
{
    public class CustomErrorModel : PageModel
    {
        public int StatusCode { get; set; }

        public void OnGet(int statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
