using NLog;
using SnackBar.BLL.Interfaces;
using SnackBar.BLL.Models;
using SnackBar.DAL.Interfaces;
using SnackBar.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SnackBar.BLL.Services
{
    public class PaymentService : BaseService, IPaymentService
    {
        private Logger _logger = LogManager.GetLogger("Payments");

        public PaymentService(IDalFactory factory) : base(factory)
        {
        }

        public Payment AddNewPayment(string email, decimal money)
        {
            if (string.IsNullOrEmpty(email))
            {
                _logger.Warn($"Incorrect Email (NULL or Empty)");
                return null;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                uow.BeginTransaction();
                var user = uow.Repository<Buyer>()
                            .Find(u => !u.IsDeleted)
                            .FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                if (user == null)
                {
                    _logger.Warn($"User(Email=\"{email}\") not found");
                    return null;
                }

                var payment = new Payment
                {
                    Date = DateTime.Now,
                    Money = money,
                    UserId = user.Id
                };

                user.Balance += money;
                uow.Repository<Buyer>().AddOrUpdate(user);
                var returnResult = uow.Repository<Payment>().AddOrUpdate(payment);

                try
                {
                    uow.Save();
                    uow.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Warn($"DB Error while saving payment EMAIL={email}. Exception Message: {ex.Message} ");
                    return null;
                }

                return returnResult;
            });
        }

        public IEnumerable<LightPayment> GetLastPayments(Expression<Func<Payment, bool>> filter = null)
        {
            return this.InvokeInUnitOfWorkScope(uow =>
            {
                if (filter == null)
                {
                    return uow.Repository<Payment>()
                        .Find(p => !p.IsDeleted, (p => p.User))
                        .Select(x => new LightPayment { Money = x.Money, Email = x.User.Email, Date = x.Date })
                        .OrderByDescending(o => o.Date)
                        .Take(10)
                        .ToList();
                }

                return uow.Repository<Payment>()
                    .Find(filter, (p => p.User))
                    .Where(o => !o.IsDeleted)
                    .Select(x => new LightPayment { Money = x.Money, Email = x.User.Email, Date = x.Date })
                    .OrderByDescending(o => o.Date)
                    .Take(10)
                    .ToList();
            });
        }

        public IEnumerable<LightPayment> GetAllPayments(Expression<Func<Payment, bool>> filter = null)
        {
            return this.InvokeInUnitOfWorkScope(uow =>
            {
                if (filter == null)
                {
                    return uow.Repository<Payment>()
                        .Find(p => !p.IsDeleted, (p => p.User))
                        .Select(x => new LightPayment { Money = x.Money, Email = x.User.Email, Date = x.Date })
                        .OrderByDescending(o => o.Date)
                        .ToList();
                }

                return uow.Repository<Payment>()
                    .Find(filter, (p => p.User))
                    .Where(o => !o.IsDeleted)
                    .Select(x => new LightPayment { Money = x.Money, Email = x.User.Email, Date = x.Date })
                    .OrderByDescending(o => o.Date)
                    .ToList();
            });
        }

        public IEnumerable<LightPayment> GetAllUserPayments(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                _logger.Warn($"User(Email=\"{email}\") not found");
                return Enumerable.Empty<LightPayment>();
            }

            return this.GetAllPayments(p => p.User.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public Payment GetPayment(long paymentId)
        {
            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var payment = uow.Repository<Payment>().GetById(paymentId, (p => p.User));
                return payment;
            });
        }
    }
}