using System;

namespace SnackBar.DAL.Models
{
    /// <summary>
    /// Order model
    /// </summary>
    public class Order : BaseModel
    {
        /// <summary>
        /// Product ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// User's PIN
        /// </summary>
        public string Pin { get; set; }

        /// <summary>
        /// Price for 1 pcs
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// DateTime of order
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Count of products
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// Total money for products
        /// </summary>
        public decimal TotalPrice
        {
            get
            {
                int count = this.Count ?? 0;
                return count > 0 ? this.Price * count : this.Price;
            }
        }

        /// <summary>
        /// Ordered product
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public virtual Buyer User { get; set; }
    }
}