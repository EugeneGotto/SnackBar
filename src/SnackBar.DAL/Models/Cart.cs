using System.Collections.Generic;
using System.Linq;

namespace SnackBar.DAL.Models
{
    /// <summary>
    /// Cart model
    /// </summary>
    public class Cart
    {
        private List<CartItem> _cartCollection;

        /// <summary>
        /// List of Products in cart
        /// </summary>
        public IEnumerable<CartItem> CartCollection
        {
            get
            {
                if (this._cartCollection == null)
                {
                    this._cartCollection = new List<CartItem>();
                }

                return this._cartCollection;
            }
            set
            {
                if (value == null)
                {
                    this._cartCollection.Clear();
                }

                this._cartCollection = value.ToList();
            }
        }

        /// <summary>
        /// Total Items in Cart
        /// </summary>
        public int TotalItems
        {
            get { return this.CartCollection.Sum(c => c.Count); }
        }

        /// <summary>
        /// Total money for products in cart
        /// </summary>
        public decimal TotalCartPrice
        {
            get { return this.CartCollection.Sum(c => c.TotalPrice); }
        }
    }
}