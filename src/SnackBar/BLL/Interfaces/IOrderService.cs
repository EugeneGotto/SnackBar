using SnackBar.BLL.Models;
using SnackBar.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SnackBar.BLL.Interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// Add to DB new Order
        /// </summary>
        /// <param name="order">Added Order</param>
        Order AddNewOrder(Order order);

        /// <summary>
        /// Get All Orders
        /// </summary>
        /// <returns>Orders</returns>
        IEnumerable<LightOrder> GetAllOrders(Expression<Func<Order, bool>> filter = null);

        /// <summary>
        /// Get All Today Orders
        /// </summary>
        /// <returns>Today Orders</returns>
        IEnumerable<LightOrder> GetAllTodayOrders();

        /// <summary>
        /// Get All Month Orders
        /// </summary>
        /// <returns>Today Orders</returns>
        IEnumerable<LightOrder> GetAllMonthOrders();

        /// <summary>
        /// Get All Last Month Orders
        /// </summary>
        /// <returns>Today Orders</returns>
        IEnumerable<LightOrder> GetAllLastMonthOrders();

        /// <summary>
        /// Get All User's Orders
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>User's Orders</returns>
        IEnumerable<LightOrder> GetAllUserOrders(string email);

        /// <summary>
        /// Get 20 Last Orders
        /// </summary>
        /// <param name="pin">User PIN</param>
        /// <returns>User's orders</returns>
        IEnumerable<LightOrder> GetLastUserOrders(string pin);

        /// <summary>
        /// Get User Order by ID
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns>User's Order</returns>
        Order GetOrderById(long orderId);

        /// <summary>
        /// Delete Order
        /// </summary>
        /// <param name="orderId">Deleted Order ID</param>
        void DeleteOrder(long orderId);
    }
}