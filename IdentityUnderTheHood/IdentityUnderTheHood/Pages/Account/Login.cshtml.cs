using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Security.Claims;

namespace IdentityUnderTheHood.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; } = new Credential();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync() 
        {
            if (!ModelState.IsValid) return Page();
            if (Credential.Username == "admin" && Credential.Password == "password123") 
            {
                var claims = new List<Claim>
                {
                    new Claim( ClaimTypes.Name, "admin"),
                    new Claim( ClaimTypes.Email, "admin@mywebsite.com"),
                    new Claim( "Department", "HR"),
                    new Claim( "Admin", "true"),
                    new Claim( "Management", "true"), 
                    new Claim( "EmploymentDate", "2023-05-01")
                };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Credential.RememberMe
                };

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);

                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
