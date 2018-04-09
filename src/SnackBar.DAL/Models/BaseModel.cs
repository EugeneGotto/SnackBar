namespace SnackBar.DAL.Models
{
    public abstract class BaseModel
    {
        /// <summary>
        /// Item ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Soft Delete Flag
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}