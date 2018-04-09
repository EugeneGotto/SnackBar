namespace SnackBar.DAL.Models
{
    /// <summary>
    /// Buyer (User) Model
    /// </summary>
    public class Buyer : BaseModel
    {
        /// <summary>
        /// User's Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's unique PIN
        /// </summary>
        public string Pin { get; set; }

        /// <summary>
        /// User's actual Balance
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// User's Password (not realized now)
        /// </summary>
        public string Password { get; set; }
    }
}