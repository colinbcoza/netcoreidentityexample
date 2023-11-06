using System.ComponentModel.DataAnnotations;

namespace IdentityUnderTheHood.Pages.Account
{
    public class Credential
    {
        [Required]
        [Display(Description = "User Name")]
        public string  Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Description = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}