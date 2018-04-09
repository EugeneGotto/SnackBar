using System.Collections.Generic;

namespace SnackBar.ViewModels
{
    /// <summary>
    /// Summary Buyer's Information
    /// </summary>
    public class UserIndexPageViewModel
    {
        /// <summary>
        /// Buyer Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Actual balance
        /// </summary>
        public decimal? Balance { get; set; }

        /// <summary>
        /// Last 5 orders
        /// </summary>
        public IList<OrderUserListViewModel> Orders { get; set; }
    }
}