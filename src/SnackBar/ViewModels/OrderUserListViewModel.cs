using System;

namespace SnackBar.ViewModels
{
    /// <summary>
    /// User's Order
    /// </summary>
    public class OrderUserListViewModel
    {
        /// <summary>
        /// Product name
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Count of ordered products
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Total price of order
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Order creation time
        /// </summary>
        public DateTime Date { get; set; }
    }
}