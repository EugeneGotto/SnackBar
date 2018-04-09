using System.Collections.Generic;
using System.Linq;

namespace SnackBar.ViewModels
{
    /// <summary>
    /// Cart ViewModel for API
    /// </summary>
    public class CartApiViewModel
    {
        /// <summary>
        /// Collection of ordered items
        /// </summary>
        public IEnumerable<CartItemViewModel> CartCollection
        {
            get; set;
        }

        /// <summary>
        /// Total items in Cart
        /// </summary>
        public int TotalItems
        {
            get { return this.CartCollection.Sum(c => c.OrderedCount); }
        }

        /// <summary>
        /// Total price of ordered items
        /// </summary>
        public decimal TotalCartPrice
        {
            get { return this.CartCollection.Sum(c => c.TotalPrice); }
        }
    }

    /// <summary>
    /// Cart Item ViewModel for API
    /// </summary>
    public class CartItemViewModel
    {
        /// <summary>
        /// Cart Item ID
        /// </summary>
        public long Id
        {
            get; set;
        }

        /// <summary>
        /// Product Name
        /// </summary>
        public string ProductName
        {
            get; set;
        }

        /// <summary>
        /// Count of Product in Bar
        /// </summary>
        public string AvailableCount
        {
            get; set;
        }

        /// <summary>
        /// Price for 1 pcs.
        /// </summary>
        public decimal Price
        {
            get; set;
        }

        /// <summary>
        /// Count of product in Cart
        /// </summary>
        public int OrderedCount
        {
            get; set;
        }

        /// <summary>
        /// Total price
        /// </summary>
        public decimal TotalPrice
        {
            get { return this.Price * this.OrderedCount; }
        }
    }
}