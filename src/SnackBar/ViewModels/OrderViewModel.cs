namespace SnackBar.ViewModels
{
    /// <summary>
    /// Model for new order
    /// </summary>
    public class OrderViewModel
    {
        /// <summary>
        /// Product ID for buying
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// Buyer's PIN
        /// </summary>
        public string Pin { get; set; }
    }
}