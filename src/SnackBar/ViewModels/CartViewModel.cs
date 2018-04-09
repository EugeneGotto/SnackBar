using SnackBar.DAL.Models;

namespace SnackBar.ViewModels
{
    public class CartViewModel
    {
        public string BuyerName
        {
            get; set;
        }

        public decimal Balance
        {
            get; set;
        }

        public Cart Cart
        {
            get; set;
        }
    }
}