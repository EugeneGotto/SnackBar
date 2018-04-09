using SnackBar.BLL.Models;
using SnackBar.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SnackBar.BLL.Interfaces
{
    public interface IPaymentService
    {
        /// <summary>
        /// Get Payment
        /// </summary>
        /// <param name="paymentId">Payment ID</param>
        /// <returns>Payment</returns>
        Payment GetPayment(long paymentId);

        /// <summary>
        /// Get All User's Payments
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Payments</returns>
        IEnumerable<LightPayment> GetAllUserPayments(string email);

        /// <summary>
        /// Get All Payments
        /// </summary>
        /// <param name="filter">Expression Filter</param>
        /// <returns>All Payments</returns>
        IEnumerable<LightPayment> GetAllPayments(Expression<Func<Payment, bool>> filter = null);

        /// <summary>
        /// Get 10 Last Payments
        /// </summary>
        /// <param name="filter">Expression Filter</param>
        /// <returns>Last Payments</returns>
        IEnumerable<LightPayment> GetLastPayments(Expression<Func<Payment, bool>> filter = null);

        /// <summary>
        /// Add new Payment
        /// </summary>
        /// <param name="payment">Added Payment</param>
        Payment AddNewPayment(string userEmail, decimal money);
    }
}