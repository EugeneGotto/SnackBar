namespace SnackBar.DAL.Models
{
    /// <summary>
    /// Cart Item model
    /// </summary>
    public class CartItem : BaseModel
    {
        /// <summary>
        /// Product ID
        /// </summary>
        public long ProductId
        {
            get; set;
        }

        /// <summary>
        /// Product model
        /// </summary>
        public virtual Product Product
        {
            get; set;
        }

        /// <summary>
        /// User ID
        /// </summary>
        public long BuyerId
        {
            get; set;
        }

        /// <summary>
        /// User
        /// </summary>
        public virtual Buyer Buyer
        {
            get; set;
        }

        /// <summary>
        /// Count of product
        /// </summary>
        public int Count
        {
            get; set;
        }

        /// <summary>
        /// Total price of Products
        /// </summary>
        public decimal TotalPrice
        {
            get { return this.Product.Price * this.Count; }
        }
    }
}