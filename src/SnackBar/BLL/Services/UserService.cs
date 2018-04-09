using Microsoft.AspNet.Identity;
using NLog;
using SnackBar.BLL.Interfaces;
using SnackBar.BLL.Utils;
using SnackBar.DAL.Interfaces;
using SnackBar.DAL.Models;
using SnackBar.ViewModels;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SnackBar.BLL.Services
{
    public class UserService : BaseService, IUserService
    {
        private Logger _logger = LogManager.GetLogger("Users");

        public UserService(IDalFactory factory) : base(factory)
        {
        }

        public decimal? GetBalanceById(int userId)
        {
            if (userId <= 0)
            {
                return null;
            }

            return this.GetUserById(userId).Balance;
        }

        public decimal? GetBalanceByPin(string pin)
        {
            return this.InvokeInUnitOfWorkScope<decimal?>(uow =>
            {
                var user = uow.Repository<Buyer>()
                    .Find(u => !u.IsDeleted)
                    .FirstOrDefault(u => u.Pin.Equals(pin, StringComparison.InvariantCulture));

                if (user == null)
                {
                    _logger.Warn($"BalanceByPIN: User(PIN=\"{pin}\") not found");
                    return null;
                }

                return user.Balance;
            });
        }

        public decimal? GetBalanceByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return this.InvokeInUnitOfWorkScope<decimal?>(uow =>
            {
                var user = uow.Repository<Buyer>()
                    .Find(u => !u.IsDeleted)
                    .FirstOrDefault(u => u.Email.ToUpper().Equals(email.ToUpper(), StringComparison.OrdinalIgnoreCase));

                if (user == null)
                {
                    return null;
                }

                return user.Balance;
            });
        }

        public UserIndexPageViewModel GetIndexPageInfo(string pin)
        {
            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var returnResult = new UserIndexPageViewModel();

                var user = uow.Repository<Buyer>()
                    .Find(u => !u.IsDeleted)
                    .FirstOrDefault(u => u.Pin.Equals(pin, StringComparison.InvariantCulture));

                if (user == null)
                {
                    _logger.Warn($"IndexPage: User(PIN=\"{pin}\") not found");
                    return null;
                }

                returnResult.Balance = user.Balance;

                var address = new MailAddress(user.Email);

                returnResult.Name = address.User;

                var orders = uow.Repository<Order>()
                    .Find(o => !o.IsDeleted, (o => o.Product))
                    .Where(o => o.UserId == user.Id)
                    .Select(o => new OrderUserListViewModel()
                    {
                        ProductName = o.Product.Name,
                        Date = o.Date,
                        Count = (o.Count == null) || (o.Count == 0) ? 1 : (int)o.Count,
                        Price = (o.Count == null) || (o.Count == 0) ? o.Price : o.Price * (int)o.Count,
                    })
                    .OrderByDescending(o => o.Date)
                    .Take(5)
                    .ToList();

                returnResult.Orders = orders;

                return returnResult;
            });
        }

        public Buyer GetUserById(int userId)
        {
            if (userId <= 0)
            {
                return null;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var user = uow.Repository<Buyer>()
                      .GetById(userId);

                return user.IsDeleted ? null : user;
            });
        }

        public long GetUserIdByEmail(string email)
        {
            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var user = uow.Repository<Buyer>().FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                if (user == null)
                {
                    _logger.Warn($"UserByEmail: User(Email=\"{email}\") not found");
                    return 0;
                }

                return user.Id;
            });
        }

        public long GetUserIdByPin(string pin)
        {
            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var user = uow.Repository<Buyer>().FirstOrDefault(u => u.Pin.Equals(pin, StringComparison.InvariantCulture));

                if (user == null)
                {
                    _logger.Warn($"UserByPIN: User(PIN=\"{pin}\") not found");
                    return 0;
                }

                return user.Id;
            });
        }

        public async Task<Buyer> RegisterUser(string email)
        {
            using (var uow = _dalfactory.GetUnitOfWork())
            {
                var user = uow.Repository<Buyer>()
                    .FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                string pin;
                var client = new EmailService();
                IdentityMessage message;

                if (user == null)
                {
                    _logger.Info("User not found");
                    do
                    {
                        pin = PinGenerator.RandomString(4);
                    }
                    while (uow.Repository<Buyer>().Exist(u => u.Pin.Equals(pin)));

                    user = new Buyer()
                    {
                        Balance = 0,
                        Email = email,
                        Pin = pin,
                        IsDeleted = false
                    };

                    message = new IdentityMessage()
                    {
                        Body = $"Hello. Your PIN is {pin}. Use this to shoping in {ConfigurationManager.AppSettings["CompanyName"]} SnackBar.",
                        Subject = "New PIN",
                        Destination = email
                    };

                    user = uow.Repository<Buyer>().AddOrUpdate(user);
                    uow.Save();
                    _logger.Info($"User Created with PIN = {user.Pin}");
                }
                else
                {
                    pin = user.Pin;
                    message = new IdentityMessage()
                    {
                        Body = $"Hello. Your PIN is {pin}.",
                        Subject = "Forgot PIN?",
                        Destination = email
                    };
                    _logger.Info("User Found");
                }

                await client.SendAsync(message).ConfigureAwait(false);

                return user;
            }
        }

        public MailAddress GetUserEmailByPin(string pin)
        {
            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var user = uow.Repository<Buyer>().FirstOrDefault(u => u.Pin.Equals(pin, StringComparison.InvariantCulture));

                if (user == null)
                {
                    _logger.Warn($"EmailByPIN: User(PIN=\"{pin}\") not found");
                    return null;
                }

                return new MailAddress(user.Email);
            });
        }
    }
}