namespace SnackBar.ViewModels
{
    /// <summary>
    /// Present information about Tag
    /// </summary>
    public class TagViewModel
    {
        /// <summary>
        /// Tag ID
        /// </summary>
        public long Id
        {
            get; set;
        }

        /// <summary>
        /// Tag Name at English
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// Tag Name at Russian
        /// </summary>
        public string NameRu
        {
            get; set;
        }

        /// <summary>
        /// Count of available Products
        /// </summary>
        public int Count
        {
            get; set;
        }
    }
}