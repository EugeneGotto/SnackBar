using SnackBar.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace SnackBar.ViewModels
{
    /// <summary>
    /// Check after Success Order
    /// </summary>
    public class CheckViewModel
    {
        private List<OrderedItem> _collection;

        /// <summary>
        /// Actual Balance
        /// </summary>
        public decimal? Balance
        {
            get; set;
        }

        /// <summary>
        /// Check Lines of Purchased products
        /// </summary>
        public IList<OrderedItem> CheckLines
        {
            get
            {
                if (this._collection == null)
                {
                    this._collection = new List<OrderedItem>();
                }

                return this._collection;
            }
            set
            {
                if (value == null)
                {
                    this._collection?.Clear();
                }

                this._collection = value.ToList();
            }
        }

        /// <summary>
        /// Total Purchased products
        /// </summary>
        public int TotalPurchasedCount
        {
            get { return this.CheckLines.Sum(c => c.PurchasedCount); }
        }

        /// <summary>
        /// Total Sum
        /// </summary>
        public decimal TotalMoney
        {
            get { return this.CheckLines.Sum(c => c.PriceOnePcs * c.PurchasedCount); }
        }
    }
}