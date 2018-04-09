using SnackBar.DAL.Models;
using System.Collections.Generic;

namespace SnackBar.BLL.Interfaces
{
    public interface IAdminService
    {
        /// <summary>
        /// Change the count of available Product in Bar
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="newCount">Product new Count</param>
        Product ChangeProductCount(int productId, byte newCount);

        /// <summary>
        /// Change Product's price
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="newPrice">New Price</param>
        Product ChangeProductPrice(int productId, decimal newPrice);

        /// <summary>
        /// Change User Balance (increase/decrease)
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="money">Changed sum (positive if increase, negative if decrease)</param>
        decimal? ChangeUserBalance(int userId, decimal money);

        /// <summary>
        /// Get all Users with negative Balance
        /// </summary>
        /// <returns>Users</returns>
        IEnumerable<Buyer> GetAllDebitors();
    }
}