using System.ComponentModel.DataAnnotations;

namespace SnackBar.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof(Resx.Resource))]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resx.Resource))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(Resx.Resource))]
        public bool RememberMe { get; set; }
    }
}