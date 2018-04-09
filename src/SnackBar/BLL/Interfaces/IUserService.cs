using SnackBar.DAL.Models;
using SnackBar.ViewModels;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SnackBar.BLL.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Add new User
        /// </summary>
        /// <param name="email">User's Email</param>
        /// <returns>User's PIN</returns>
        Task<Buyer> RegisterUser(string email);

        /// <summary>
        /// Get User's Balance By Id
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>User's Balance</returns>
        decimal? GetBalanceById(int userId);

        /// <summary>
        /// Get User's Balance By PIN
        /// </summary>
        /// <param name="pin">User PIN</param>
        /// <returns>User's Balance</returns>
        decimal? GetBalanceByPin(string pin);

        /// <summary>
        /// get User's Balance By Email
        /// </summary>
        /// <param name="email">User Email</param>
        /// <returns>User's Balance</returns>
        decimal? GetBalanceByEmail(string email);

        /// <summary>
        /// Get User's ID by Email
        /// </summary>
        /// <param name="email">User Email</param>
        /// <returns>User ID</returns>
        long GetUserIdByEmail(string email);

        /// <summary>
        /// Get User's ID by PIN
        /// </summary>
        /// <param name="pin">User PIN</param>
        /// <returns>User's ID</returns>
        long GetUserIdByPin(string pin);

        /// <summary>
        /// Get User by ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>User</returns>
        Buyer GetUserById(int userId);

        /// <summary>
        /// Get info for Index View by user PIN
        /// </summary>
        /// <param name="pin">User PIN</param>
        /// <returns>User info view model</returns>
        UserIndexPageViewModel GetIndexPageInfo(string pin);

        /// <summary>
        /// Get User's Email by PIN
        /// </summary>
        /// <param name="pin">User PIN</param>
        /// <returns>User Mail Address</returns>
        MailAddress GetUserEmailByPin(string pin);
    }
}