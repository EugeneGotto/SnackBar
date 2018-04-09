using System;

namespace SnackBar.ViewModels
{
    /// <summary>
    /// Payment ViewModel
    /// </summary>
    public class PaymentListViewModel
    {
        /// <summary>
        /// User Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Money to payment (negative if reduce balance)
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// DateTime of Payment
        /// </summary>
        public DateTime Date { get; set; }
    }
}