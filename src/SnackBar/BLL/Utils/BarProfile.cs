using AutoMapper;
using SnackBar.BLL.Models;
using SnackBar.DAL.Models;
using SnackBar.ViewModels;

namespace SnackBar.BLL.Utils
{
    public class BarProfile : Profile
    {
        public BarProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(
                    nameof(ProductViewModel.TagStringEn),
                        opt => opt.MapFrom(src => src.TagStringEn ?? string.Empty))
                .ForMember(
                    nameof(ProductViewModel.TagStringRu),
                        opt => opt.MapFrom(src => src.TagStringRu ?? string.Empty));

            CreateMap<Product, ProductEditViewModel>()
                .ForMember(
                    nameof(ProductEditViewModel.Price),
                        opt => opt.MapFrom(src => src.Price.ToString()));

            CreateMap<ProductEditViewModel, Product>()
                .ForMember(
                    nameof(Product.Price),
                        opt => opt.MapFrom(src => decimal.Parse(src.Price)));

            CreateMap<Product, ProductApiViewModel>();

            CreateMap<Product, ProductPostViewModel>();

            CreateMap<LightPayment, PaymentListViewModel>();

            CreateMap<LightPayment, PaymentApiViewModel>();

            CreateMap<Buyer, UserViewModel>();

            CreateMap<Buyer, UserApiViewModel>();

            CreateMap<OrderViewModel, Order>();

            CreateMap<LightOrder, OrderListViewModel>();

            CreateMap<LightOrder, OrderApiViewModel>();

            CreateMap<Order, OrderUserListViewModel>()
                .ForMember(
                    nameof(OrderUserListViewModel.ProductName),
                        opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<Cart, CartApiViewModel>();
            CreateMap<CartItem, CartItemViewModel>()
                .ForMember(
                    nameof(CartItemViewModel.Id),
                        opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    nameof(CartItemViewModel.ProductName),
                        opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(
                    nameof(CartItemViewModel.AvailableCount),
                        opt => opt.MapFrom(src => src.Product.Count))
                .ForMember(
                    nameof(CartItemViewModel.Price),
                        opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(
                    nameof(CartItemViewModel.OrderedCount),
                        opt => opt.MapFrom(src => src.Count));

            CreateMap<Tag, TagViewModel>();
        }
    }
}