using SnackBar.BLL.Interfaces;
using SnackBar.DAL.Interfaces;
using SnackBar.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace SnackBar.BLL.Services
{
    public class AdminService : BaseService, IAdminService
    {
        public AdminService(IDalFactory factory) : base(factory)
        {
        }

        public Product ChangeProductCount(int productId, byte newCount)
        {
            if (productId <= 0)
            {
                return null;
            }

            using (var uow = _dalfactory.GetUnitOfWork())
            {
                var product = uow.Repository<Product>().GetById(productId);
                if (product == null)
                {
                    return null;
                }

                product.Count = newCount;
                uow.Repository<Product>().AddOrUpdate(product);
                uow.Save();
                return product;
            }
        }

        public Product ChangeProductPrice(int productId, decimal newPrice)
        {
            if (productId <= 0)
            {
                return null;
            }

            using (var uow = _dalfactory.GetUnitOfWork())
            {
                var product = uow.Repository<Product>().GetById(productId);
                if (product == null)
                {
                    return null;
                }

                product.Price = newPrice;
                uow.Repository<Product>().AddOrUpdate(product);
                uow.Save();
                return product;
            }
        }

        public decimal? ChangeUserBalance(int userId, decimal money)
        {
            if (userId <= 0)
            {
                return null;
            }

            using (var uow = _dalfactory.GetUnitOfWork())
            {
                var user = uow.Repository<Buyer>().GetById(userId);
                if (user == null)
                {
                    return null;
                }

                user.Balance += money;
                uow.Repository<Buyer>().AddOrUpdate(user);
                uow.Save();

                return user.Balance;
            }
        }

        public IEnumerable<Buyer> GetAllDebitors()
        {
            using (var uow = _dalfactory.GetUnitOfWork())
            {
                return uow.Repository<Buyer>()
                    .Find(u => !u.IsDeleted)
                    .Where(u => u.Balance < 0)
                    .OrderBy(u => u.Email)
                    .ToList();
            }
        }
    }
}