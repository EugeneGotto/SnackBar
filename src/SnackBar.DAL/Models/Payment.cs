using System;

namespace SnackBar.DAL.Models
{
    /// <summary>
    /// Payment Model
    /// </summary>
    public class Payment : BaseModel
    {
        /// <summary>
        /// User ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// DateTime of Payment
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Money of Payment
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public virtual Buyer User { get; set; }
    }
}