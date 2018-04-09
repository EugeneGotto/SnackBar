using NLog;
using SnackBar.BLL.Interfaces;
using SnackBar.DAL.Interfaces;
using SnackBar.DAL.Models;
using SnackBar.ViewModels;
using System;
using System.Linq;

namespace SnackBar.BLL.Services
{
    public class CartService : BaseService, ICartService
    {
        private Logger _logger = LogManager.GetLogger("Cart");

        public CartService(IDalFactory factory) : base(factory)
        {
        }

        public Cart AddItem(CartItem item)
        {
            if (item == null)
            {
                _logger.Warn("Incorrect cartItem model (NULL) in AddItem");
                return null;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var cartItem = uow.Repository<CartItem>().Find(c => c.BuyerId == item.BuyerId).FirstOrDefault(c => c.ProductId == item.ProductId);
                if (cartItem != null)
                {
                    return this.ChangeQuantity(item.Id, item.Count);
                }

                var buyer = uow.Repository<Buyer>().GetById(cartItem.BuyerId);
                if (buyer == null)
                {
                    _logger.Warn($"Buyer with ID={cartItem.BuyerId} not found");
                    return null;
                }

                var product = uow.Repository<Product>().GetById(cartItem.ProductId);
                if (product == null || product.IsDeleted)
                {
                    _logger.Warn($"Product ID={cartItem.ProductId} not found or SoftDeleted");
                    return null;
                }

                if (product.Count <= 0)
                {
                    return this.GetCartById(cartItem.BuyerId);
                }

                uow.BeginTransaction();
                uow.Repository<CartItem>().AddOrUpdate(item);

                try
                {
                    uow.Save();
                    uow.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Warn($"DB Error while adding item. PIN={buyer.Pin}. Exception Message: {ex.Message} ");
                    return null;
                }

                return this.GetCartById(item.BuyerId);
            });
        }

        public Cart AddItem(string pin, long productId)
        {
            if (string.IsNullOrEmpty(pin))
            {
                _logger.Warn("Incorrect PIN in AddItem");
                return null;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
           {
               var buyer = uow.Repository<Buyer>().FirstOrDefault(b => b.Pin.ToUpper().Equals(pin.ToUpper(), StringComparison.OrdinalIgnoreCase));
               if (buyer == null)
               {
                   _logger.Warn($"Buyer with PIN={pin} not found");
                   return null;
               }

               var cartItem = uow.Repository<CartItem>().Find(c => c.BuyerId == buyer.Id).FirstOrDefault(c => c.ProductId == productId);
               if (cartItem != null)
               {
                   return this.GetCartById(buyer.Id);
               }

               var product = uow.Repository<Product>().GetById(productId);
               if (product == null || product.IsDeleted)
               {
                   _logger.Warn($"Product ID={productId} not found or SoftDeleted");
                   return null;
               }

               if (product.Count <= 0)
               {
                   return this.GetCartById(buyer.Id);
               }

               cartItem = new CartItem()
               {
                   BuyerId = buyer.Id,
                   ProductId = productId,
                   Count = 1,
               };

               uow.BeginTransaction();
               uow.Repository<CartItem>().AddOrUpdate(cartItem);

               try
               {
                   uow.Save();
                   uow.Commit();
               }
               catch (Exception ex)
               {
                   _logger.Warn($"DB Error while adding item. PIN={buyer.Pin}. Exception Message: {ex.Message} ");
                   return null;
               }

               return this.GetCartById(buyer.Id);
           });
        }

        public CheckViewModel MakeOrder(string pin)
        {
            if (string.IsNullOrEmpty(pin))
            {
                _logger.Warn("Incorrect Pin in MakeOrder");
                return null;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var buyer = uow.Repository<Buyer>().FirstOrDefault(b => b.Pin.ToUpper().Equals(pin.ToUpper(), StringComparison.OrdinalIgnoreCase));
                if (buyer == null)
                {
                    _logger.Warn($"Buyer with PIN={pin} not found");
                    return null;
                }

                var cart = this.GetCartById(buyer.Id);
                var result = new CheckViewModel();

                if (!cart.CartCollection.Any())
                {
                    result.Balance = buyer.Balance;
                    return result;
                }

                uow.BeginTransaction();
                foreach (var item in cart.CartCollection)
                {
                    var ordereditem = this.AddOrderFromCartItem(item, ref uow, ref buyer);
                    if (ordereditem != null)
                    {
                        result.CheckLines.Add(ordereditem);
                    }
                }

                buyer = uow.Repository<Buyer>().AddOrUpdate(buyer);

                try
                {
                    uow.Save();
                    uow.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Warn($"DB Error while saving order PIN={buyer.Pin}. Exception Message: {ex.Message} ");
                    return null;
                }

                result.Balance = buyer.Balance;

                return result;
            });
        }

        public Cart RemoveItem(long cartItemId)
        {
            if (cartItemId <= 0)
            {
                _logger.Warn($"Incorrect cartItem ID={cartItemId} in RemoveItem");
                return null;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var cartItem = uow.Repository<CartItem>().GetById(cartItemId);
                if (cartItem == null)
                {
                    return null;
                }

                var buyerId = cartItem.BuyerId;
                uow.BeginTransaction();
                uow.Repository<CartItem>().Delete(cartItem);
                try
                {
                    uow.Save();
                    uow.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Warn($"DB Error while removing item. Exception Message: {ex.Message} ");
                    return null;
                }

                return this.GetCartById(buyerId);
            });
        }

        public Cart ChangeQuantity(long cartItemId, int quantity)
        {
            if (cartItemId <= 0 || quantity < 0)
            {
                _logger.Warn($"Incorrect cartItem ID={cartItemId} or quantity of product are negative");
            }

            if (quantity == 0)
            {
                return this.RemoveItem(cartItemId);
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var cartItem = uow.Repository<CartItem>().GetById(cartItemId);
                if (cartItem == null)
                {
                    _logger.Warn($"CartItem ID={cartItemId} not found");
                    return null;
                }

                cartItem.Count = quantity;

                if (cartItem.Count > cartItem.Product.Count)
                {
                    cartItem.Count = cartItem.Product.Count;
                }

                if (cartItem.Count <= 0)
                {
                    cartItem.Count = 1;
                }

                cartItem = uow.Repository<CartItem>().AddOrUpdate(cartItem);
                uow.Save();

                return this.GetCartById(cartItem.BuyerId);
            });
        }

        public Cart InDeCreaseQuantity(long cartItemId, int changeTo)
        {
            if (cartItemId <= 0)
            {
                _logger.Warn($"Incorrect cartItem ID={cartItemId} or quantity of product are negative");
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var cartItem = uow.Repository<CartItem>().GetById(cartItemId);
                if (cartItem == null)
                {
                    _logger.Warn($"CartItem ID={cartItemId} not found");
                    return null;
                }

                cartItem.Count += changeTo;

                if (cartItem.Count > cartItem.Product.Count)
                {
                    cartItem.Count = cartItem.Product.Count;
                }

                if (cartItem.Count <= 0)
                {
                    cartItem.Count = 1;
                }

                cartItem = uow.Repository<CartItem>().AddOrUpdate(cartItem);
                uow.Save();

                return this.GetCartById(cartItem.BuyerId);
            });
        }

        public Cart Clear(string pin)
        {
            if (string.IsNullOrEmpty(pin))
            {
                _logger.Warn("Incorrect Pin in Clear");
                return null;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var cart = this.GetCartByPin(pin);

                foreach (var cartItem in cart.CartCollection)
                {
                    uow.Repository<CartItem>().Delete(cartItem);
                }

                uow.Save();
                return new Cart();
            });
        }

        public Cart GetCartByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                _logger.Warn("Incorrect Email in GetCart");
                return null;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var cart = new Cart();
                var cartItems = uow.Repository<CartItem>()
                    .Find(c => !c.IsDeleted, (c => c.Buyer), (c => c.Product))
                    .Where(c => c.Buyer.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                cart.CartCollection = cartItems;

                return cart;
            });
        }

        public Cart GetCartById(long buyerId)
        {
            if (buyerId <= 0)
            {
                _logger.Warn($"Incorrect Buyer ID={buyerId}");
                return null;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
           {
               var cart = new Cart();
               var cartItems = uow.Repository<CartItem>()
                   .Find(c => !c.IsDeleted, (c => c.Buyer), (c => c.Product))
                   .Where(c => c.BuyerId == buyerId)
                   .ToList();

               cart.CartCollection = cartItems;

               return cart;
           });
        }

        public Cart GetCartByPin(string pin)
        {
            if (string.IsNullOrEmpty(pin))
            {
                _logger.Warn("Incorrect PIN in GetCart");
                return null;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var cart = new Cart();
                var cartItems = uow.Repository<CartItem>()
                    .Find(c => !c.IsDeleted, (c => c.Buyer), (c => c.Product))
                    .Where(c => c.Buyer.Pin.Equals(pin, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                foreach (var item in cartItems)
                {
                    if (item.Product.Count < item.Count)
                    {
                        item.Count = item.Product.Count;
                    }
                }

                cart.CartCollection = cartItems;

                return cart;
            });
        }

        private OrderedItem AddOrderFromCartItem(CartItem item, ref IUnitOfWork uow, ref Buyer buyer)
        {
            var product = uow.Repository<Product>().GetById(item.ProductId);
            if (product == null)
            {
                _logger.Warn($"Product ID={item.ProductId} not found");
                return null;
            }

            if (buyer.Id != item.BuyerId)
            {
                _logger.Warn("Incorrect buyer ID in cartItem");
                return null;
            }

            var checkItem = new OrderedItem()
            {
                ProductName = product.Name,
                PriceOnePcs = product.Price,
                OrderedCount = item.Count,
                PurchasedCount = item.Count
            };

            if (product.Count <= 0)
            {
                checkItem.PurchasedCount = 0;
            }

            if (product.Count < item.Count)
            {
                checkItem.PurchasedCount = product.Count;
            }

            if (checkItem.PurchasedCount != 0)
            {
                var order = new Order()
                {
                    ProductId = product.Id,
                    Pin = buyer.Pin,
                    Count = checkItem.PurchasedCount,
                    Date = DateTime.Now,
                    Price = product.Price > 0 ? product.Price : product.Price * -1,
                    UserId = buyer.Id,
                };

                buyer.Balance -= (int)order.Count > 0 ? order.Price * (int)order.Count : order.Price;
                product.Count -= (byte)checkItem.PurchasedCount;

                uow.Repository<Product>().AddOrUpdate(product);
                order = uow.Repository<Order>().AddOrUpdate(order);
                _logger.Info($"{order.User.Email} Buy \"{order.Product.Name}\" {order.Count} pcs. for {order.TotalPrice} BYN");
            }

            uow.Repository<CartItem>().DeleteById(item.Id);
            return checkItem;
        }
    }
}