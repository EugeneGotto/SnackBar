using System.Collections.Generic;

namespace SnackBar.ViewModels
{
    public class ProductListViewModel
    {
        public List<ProductViewModel> Products { get; set; }
        public string Pin { get; set; }
        public UserIndexPageViewModel UserInfo { get; set; }
    }
}