using System;

namespace SnackBar.ViewModels
{
    /// <summary>
    /// ViewModel for Orders list
    /// </summary>
    public class OrderListViewModel
    {
        /// <summary>
        /// Name of product
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// User Email
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Price of product
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Count of Products
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Total Price of order
        /// </summary>
        public string TotalPrice
        {
            get { return (this.Price * this.Count).ToString() + " BYN"; }
        }

        /// <summary>
        /// Date of Order
        /// </summary>
        public DateTime Date { get; set; }
    }
}