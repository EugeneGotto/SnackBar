using SnackBar.DAL.Models;
using SnackBar.ViewModels;

namespace SnackBar.BLL.Interfaces
{
    public interface ICartService
    {
        Cart AddItem(CartItem item);

        Cart AddItem(string pin, long productId);

        CheckViewModel MakeOrder(string pin);

        Cart RemoveItem(long cartItemId);

        Cart ChangeQuantity(long cartItemId, int quantity);

        Cart InDeCreaseQuantity(long cartItemId, int changeTo);

        Cart Clear(string pin);

        Cart GetCartById(long buyerId);

        Cart GetCartByPin(string pin);

        Cart GetCartByEmail(string email);
    }
}