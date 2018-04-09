namespace SnackBar.ViewModels
{
    /// <summary>
    /// Added Product model
    /// </summary>
    public class ProductPostViewModel
    {
        /// <summary>
        /// Product Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product Count
        /// </summary>
        public byte Count { get; set; }

        /// <summary>
        /// Product Price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Product Barcode
        /// </summary>
        public string Barcode { get; set; }
    }
}