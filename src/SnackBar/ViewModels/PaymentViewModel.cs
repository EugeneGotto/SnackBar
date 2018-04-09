using System.ComponentModel.DataAnnotations;

namespace SnackBar.ViewModels
{
    /// <summary>
    /// Payment model for adding new payment
    /// </summary>
    public class PaymentViewModel
    {
        /// <summary>
        /// Email of User
        /// </summary>
        [Required]
        [Display(Name = "Email", ResourceType = typeof(Resx.Resource))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Money to payment (negative if reduce balance)
        /// </summary>
        [Required]
        [Display(Name = "Money", ResourceType = typeof(Resx.Resource))]
        [RegularExpression(@"^-?\d{1,3}([.,]\d{1,2})?$", ErrorMessageResourceName = "MoneyValidation", ErrorMessageResourceType = typeof(Resx.Resource))]
        public string Money { get; set; }   //Format (-) XXX.XX
    }
}