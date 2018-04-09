using System.ComponentModel.DataAnnotations;

namespace SnackBar.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(Resx.Resource))]
        public string Email { get; set; }
    }
}