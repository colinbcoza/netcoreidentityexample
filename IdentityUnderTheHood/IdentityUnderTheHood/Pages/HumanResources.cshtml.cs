using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityUnderTheHood.Pages
{
    [Authorize(Policy = "MustBelongToHRDepartment")]
    public class Human_ResourcesModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
