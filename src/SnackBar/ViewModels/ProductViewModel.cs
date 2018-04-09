namespace SnackBar.ViewModels
{
    /// <summary>
    /// Product View Model
    /// </summary>
    public class ProductViewModel
    {
        /// <summary>
        /// Product ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Product Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product Count in Bar
        /// </summary>
        public byte Count { get; set; }

        /// <summary>
        /// Product Price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// String with Tags at English
        /// </summary>
        public string TagStringEn { get; set; }

        /// <summary>
        /// String with Tags at Russian
        /// </summary>
        public string TagStringRu { get; set; }
    }
}