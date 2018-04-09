namespace SnackBar.ViewModels
{
    /// <summary>
    /// user ViewModel for API
    /// </summary>
    public class UserApiViewModel
    {
        /// <summary>
        /// User's ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// User's Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's PIN
        /// </summary>
        public string Pin { get; set; }

        /// <summary>
        /// User's Balance
        /// </summary>
        public decimal Balance { get; set; }
    }
}