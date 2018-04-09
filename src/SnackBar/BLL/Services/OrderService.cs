using NLog;
using SnackBar.BLL.Interfaces;
using SnackBar.BLL.Models;
using SnackBar.DAL.Interfaces;
using SnackBar.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace SnackBar.BLL.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private Logger _logger = LogManager.GetLogger("Orders");

        public OrderService(IDalFactory factory) : base(factory)
        {
        }

        public Order AddNewOrder(Order order)
        {
            if (order == null)
            {
                _logger.Warn("Incorrect order model (NULL) in AddNewOrder");
                return null;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                uow.BeginTransaction();
                var product = uow.Repository<Product>().GetById(order.ProductId);
                if (product == null || product.Count <= 0)
                {
                    _logger.Warn($"Incorrect Product ID({order.ProductId}) in AddNewOrder by User(PIN) - {order.Pin}.");
                    return null;
                }

                var user = uow.Repository<Buyer>()
                    .Find(u => !u.IsDeleted)
                    .FirstOrDefault(u => u.Pin.Equals(order.Pin, StringComparison.OrdinalIgnoreCase));

                if (user == null)
                {
                    _logger.Warn($"User with PIN - {order.Pin} not Found");
                    return null;
                }

                order.Price = product.Price;
                order.Date = DateTime.Now;
                order.UserId = user.Id;
                order.Count = (order.Count == null) || (order.Count == 0) ? 1 : order.Count;
                order.Price = order.Price > 0 ? order.Price : order.Price * -1;
                user.Balance -= (int)order.Count > 0 ? order.Price * (int)order.Count : order.Price;
                product.Count--;

                uow.Repository<Product>().AddOrUpdate(product);
                uow.Repository<Buyer>().AddOrUpdate(user);
                var returnResult = uow.Repository<Order>().AddOrUpdate(order);

                try
                {
                    uow.Save();
                    uow.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Warn($"DB Error while saving order. PIN={order.Pin}. Exception Message: {ex.Message} ");
                    return null;
                }

                _logger.Info($"{order.User.Email} Buy \"{order.Product.Name}\" for {order.Price} BYN");

                return returnResult;
            });
        }

        public IEnumerable<LightOrder> GetAllOrders(Expression<Func<Order, bool>> filter = null)
        {
            return this.InvokeInUnitOfWorkScope(uow =>
            {
                if (filter == null)
                {
                    return uow.Repository<Order>()
                        .Find(o => !o.IsDeleted, (o => o.Product), (o => o.User))
                        .Select(o => new LightOrder()
                        {
                            ProductName = o.Product.Name,
                            UserEmail = o.User.Email,
                            Price = o.Price,
                            Date = o.Date,
                            Count = (o.Count == null) || (o.Count == 0) ? 1 : (int)o.Count
                        })
                        .OrderByDescending(o => o.Date)
                        .ToList();
                }

                return uow.Repository<Order>()
                    .Find(o => !o.IsDeleted, (o => o.Product), (o => o.User))
                    .Where(filter)
                    .Select(o => new LightOrder()
                    {
                        ProductName = o.Product.Name,
                        UserEmail = o.User.Email,
                        Price = o.Price,
                        Date = o.Date,
                        Count = (o.Count == null) || (o.Count == 0) ? 1 : (int)o.Count
                    })
                    .OrderByDescending(o => o.Date)
                    .ToList();
            });
        }

        public IEnumerable<LightOrder> GetAllUserOrders(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Enumerable.Empty<LightOrder>();
            }

            using (var uow = _dalfactory.GetUnitOfWork())
            {
                if (!uow.Repository<Buyer>().Exist(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                {
                    return null;
                }
            }

            return this.GetAllOrders(o => o.User.Email.Equals(email, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public IEnumerable<LightOrder> GetAllTodayOrders()
        {
            var now = DateTime.Now;
            return this.GetAllOrders(o => DbFunctions.TruncateTime(o.Date) == DbFunctions.TruncateTime(now));
        }

        public IEnumerable<LightOrder> GetAllMonthOrders()
        {
            var now = DateTime.Now;
            return this.GetAllOrders(o => DbFunctions.DiffMonths(now, o.Date) == 0);
        }

        public IEnumerable<LightOrder> GetAllLastMonthOrders()
        {
            var now = DateTime.Now;
            return this.GetAllOrders(o => DbFunctions.DiffMonths(now, o.Date) == -1);
        }

        public IEnumerable<LightOrder> GetLastUserOrders(string pin)
        {
            if (string.IsNullOrEmpty(pin))
            {
                return Enumerable.Empty<LightOrder>();
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                if (!uow.Repository<Buyer>().Exist(u => u.Pin.ToUpper().Equals(pin.ToUpper(), StringComparison.OrdinalIgnoreCase)))
                {
                    return null;
                }

                return uow.Repository<Order>()
                    .Find(o => !o.IsDeleted, (o => o.Product), (o => o.User))
                    .Where(o => o.Pin.ToUpper().Equals(pin.ToUpper(), StringComparison.OrdinalIgnoreCase))
                    .Select(o => new LightOrder()
                    {
                        ProductName = o.Product.Name,
                        UserEmail = o.User.Email,
                        Price = o.Price,
                        Date = o.Date,
                        Count = (o.Count == null) || (o.Count == 0) ? 1 : (int)o.Count
                    })
                    .OrderByDescending(o => o.Date)
                    .Take(20)
                    .ToList();
            });
        }

        public Order GetOrderById(long orderId)
        {
            if (orderId <= 0)
            {
                return null;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var order = uow.Repository<Order>().GetById(orderId, (o => o.Product), (o => o.User));
                if (order.Count == 0)
                {
                    order.Count = 1;
                }

                return order;
            });
        }

        public void DeleteOrder(long orderId)
        {
            if (orderId <= 0)
            {
                return;
            }

            this.InvokeInUnitOfWorkScope(uow =>
            {
                uow.Repository<Order>().SoftDeleteById(orderId);
                uow.Save();
            });
        }
    }
}