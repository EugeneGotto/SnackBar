using NLog;
using SnackBar.BLL.Interfaces;
using SnackBar.DAL.Interfaces;
using SnackBar.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace SnackBar.BLL.Services
{
    public class ProductService : BaseService, IProductService
    {
        private Logger _logger = LogManager.GetLogger("Products");

        public ProductService(IDalFactory factory) : base(factory)
        {
        }

        public Product AddNewProduct(Product product, params long[] tagsIds)
        {
            if (product == null)
            {
                _logger.Warn("AddNew: Incorrect Product model (NULL)");
                return null;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                if (uow.Repository<Product>().Exist(p => p.Barcode.Equals(product.Barcode, StringComparison.OrdinalIgnoreCase)))
                {
                    _logger.Warn($"Product with Barcode - \"{product.Barcode}\" is already available in DB");
                    return null;
                }

                if (tagsIds != null && tagsIds.Any())
                {
                    var tags = uow.Repository<Tag>()
                    .Find(t => !t.IsDeleted)
                    .Where(t => tagsIds.Contains(t.Id))
                    .ToList();

                    if (product.Tags == null)
                    {
                        product.Tags = new List<Tag>();
                    }

                    product.Tags.Clear();
                    foreach (var tag in tags)
                    {
                        product.Tags.Add(tag);
                    }
                }

                var returnResult = uow.Repository<Product>().AddOrUpdate(product);
                uow.Save();

                return returnResult;
            });
        }

        public string DeleteProduct(long productId)
        {
            if (productId <= 0)
            {
                return string.Empty;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
           {
               var product = uow.Repository<Product>().GetById(productId);
               if (product == null)
               {
                   _logger.Warn($"In Delete: Product(ID={productId}) not found");
                   return string.Empty;
               }

               uow.Repository<Product>().SoftDeleteById(productId);
               uow.Save();
               return product.Name;
           });
        }

        public string ReturnProduct(long productId)
        {
            if (productId <= 0)
            {
                return string.Empty;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var product = uow.Repository<Product>().GetById(productId);
                if (product == null)
                {
                    _logger.Warn($"In Return: Product(ID={productId}) not found");
                    return string.Empty;
                }

                product.IsDeleted = false;
                uow.Repository<Product>().AddOrUpdate(product);
                uow.Save();

                return product.Name;
            });
        }

        public IEnumerable<Product> GetAllProducts(Expression<Func<Product, bool>> filter = null)
        {
            return this.InvokeInUnitOfWorkScope(uow =>
            {
                List<Product> result;
                if (filter == null)
                {
                    result = uow.Repository<Product>()
                        .Find(p => !p.IsDeleted)
                        .Include(p => p.Tags)
                        .OrderBy(o => o.Name)
                        .ToList();

                    return result;
                }

                result = uow.Repository<Product>()
                    .Find(filter)
                    .Where(p => !p.IsDeleted)
                    .Include(p => p.Tags)
                    .OrderBy(o => o.Name)
                    .ToList();

                return result;
            });
        }

        public IEnumerable<Product> GetAllDeletedProducts(Expression<Func<Product, bool>> filter = null)
        {
            return this.InvokeInUnitOfWorkScope(uow =>
            {
                if (filter == null)
                {
                    return uow.Repository<Product>()
                        .Find(p => p.IsDeleted)
                        .ToList();
                }

                return uow.Repository<Product>()
                    .Find(filter)
                    .Where(p => p.IsDeleted)
                    .ToList();
            });
        }

        public Product GetProduct(string barcode)
        {
            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var product = uow.Repository<Product>()
                    .FirstOrDefault(p => p.Barcode.Equals(barcode, StringComparison.OrdinalIgnoreCase));

                return product;
            });
        }

        public Product GetProductById(long productId)
        {
            if (productId <= 0)
            {
                return null;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                return uow.Repository<Product>()
                    .Find(p => !p.IsDeleted)
                    .Include(p => p.Tags)
                    .FirstOrDefault(p => p.Id == productId);
            });
        }

        public void UpdateProduct(Product product, params long[] tagsIds)
        {
            if (product == null || product.Id <= 0)
            {
                return;
            }

            this.InvokeInUnitOfWorkScope(uow =>
            {
                uow.BeginTransaction();
                var prod = uow.Repository<Product>()
                .Find(p => !p.IsDeleted)
                .Include(p => p.Tags)
                .FirstOrDefault(p => p.Id == product.Id);

                if (prod != null)
                {
                    prod.Name = product.Name;
                    prod.Count = product.Count;
                    prod.Barcode = product.Barcode;
                    prod.Price = product.Price;
                }
                else
                {
                    prod = product;
                }

                if (tagsIds != null && tagsIds.Any())
                {
                    var tags = uow.Repository<Tag>()
                    .Find(t => !t.IsDeleted)
                    .Where(t => tagsIds.Contains(t.Id))
                    .ToList();

                    if (prod.Tags == null)
                    {
                        prod.Tags = new List<Tag>();
                    }

                    prod.Tags.Clear();
                    foreach (var tag in tags)
                    {
                        prod.Tags.Add(tag);
                    }
                }

                uow.Repository<Product>().AddOrUpdate(prod);
                try
                {
                    uow.Save();
                    uow.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Warn($"DB Error while saving product (update) PIN={product.Id}. Exception Message: {ex.Message} ");
                    throw ex;
                }
            });
        }
    }
}