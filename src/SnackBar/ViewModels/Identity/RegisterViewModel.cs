using System.ComponentModel.DataAnnotations;

namespace SnackBar.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(Resx.Resource))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "ErrorPassword", ErrorMessageResourceType = typeof(Resx.Resource), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resx.Resource))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resx.Resource))]
        [Compare("Password", ErrorMessageResourceName = "ErrorComparePassword", ErrorMessageResourceType = typeof(Resx.Resource))]
        public string ConfirmPassword { get; set; }
    }
}