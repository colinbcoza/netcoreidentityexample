using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityUnderTheHood.Pages
{
    [Authorize("MustBeAdmin")]
    public class SettingsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
