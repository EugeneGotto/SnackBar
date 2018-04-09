namespace SnackBar.ViewModels
{
    /// <summary>
    /// Order ViewModel for API
    /// </summary>
    public class OrderApiViewModel : OrderListViewModel
    {
        /// <summary>
        /// Product ID in DB
        /// </summary>
        public long Id { get; set; }
    }
}