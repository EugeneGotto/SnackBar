using System.ComponentModel.DataAnnotations;

namespace SnackBar.ViewModels
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof(Resx.Resource))]
        public string Email { get; set; }
    }
}