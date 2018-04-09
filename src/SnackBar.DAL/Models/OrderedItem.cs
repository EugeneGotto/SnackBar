namespace SnackBar.DAL.Models
{
    /// <summary>
    /// Ordered Item in Check
    /// </summary>
    public class OrderedItem
    {
        /// <summary>
        /// Product Name
        /// </summary>
        public string ProductName
        {
            get; set;
        }

        /// <summary>
        /// Price for 1 pcs.
        /// </summary>
        public decimal PriceOnePcs
        {
            get; set;
        }

        /// <summary>
        /// Purchased Count of Product
        /// </summary>
        public int PurchasedCount
        {
            get; set;
        }

        /// <summary>
        /// Ordered count of Product for buying.
        /// </summary>
        public int OrderedCount
        {
            get; set;
        }

        /// <summary>
        /// Total price of Purchased Product
        /// </summary>
        public string TotalPrice
        {
            get { return (PriceOnePcs * PurchasedCount).ToString() + " BYN"; }
        }

        /// <summary>
        /// Comment if Purchased != Ordered
        /// </summary>
        public string Annotation
        {
            get
            {
                if (PurchasedCount != OrderedCount)
                {
                    return $"Quantity of \"{this.ProductName}\" in SnackBar is less";
                }

                return string.Empty;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} | {1} pcs. | {2} | {3}", this.ProductName, this.PurchasedCount, this.TotalPrice, this.Annotation);
        }
    }
}