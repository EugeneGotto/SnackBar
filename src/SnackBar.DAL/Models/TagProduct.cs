namespace SnackBar.DAL.Models
{
    /// <summary>
    /// Tag Product relations
    /// </summary>
    public class TagProduct
    {
        /// <summary>
        /// Product ID
        /// </summary>
        public long ProductId
        {
            get; set;
        }

        /// <summary>
        /// Tag ID
        /// </summary>
        public long TagId
        {
            get; set;
        }

        /// <summary>
        /// Product
        /// </summary>
        public virtual Product Product
        {
            get; set;
        }

        /// <summary>
        /// Tag
        /// </summary>
        public virtual Tag Tag
        {
            get; set;
        }
    }
}