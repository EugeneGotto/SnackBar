using Ninject;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using SnackBar.BLL.Interfaces;
using SnackBar.BLL.Services;
using SnackBar.DAL;

namespace SnackBar.BLL
{
    public class BllNinjectModule : NinjectModule
    {
        public override void Load()
        {
            if (this.Kernel != null)
            {
                this.Kernel.Load(new DalNinjectModule());
            }

            this.Bind<IAdminService>().To<AdminService>().NamedLikeFactoryMethod((IServiceFactory f) => f.GetAdminService());
            this.Bind<IOrderService>().To<OrderService>().NamedLikeFactoryMethod((IServiceFactory f) => f.GetOrderService());
            this.Bind<IPaymentService>().To<PaymentService>().NamedLikeFactoryMethod((IServiceFactory f) => f.GetPaymentService());
            this.Bind<IProductService>().To<ProductService>().NamedLikeFactoryMethod((IServiceFactory f) => f.GetProductService());
            this.Bind<IUserService>().To<UserService>().NamedLikeFactoryMethod((IServiceFactory f) => f.GetUserService());
            this.Bind<ICartService>().To<CartService>().NamedLikeFactoryMethod((IServiceFactory f) => f.GetCartService());
            this.Bind<ITagService>().To<TagService>().NamedLikeFactoryMethod((IServiceFactory f) => f.GetTagService());

            this.Bind<IServiceFactory>().ToFactory();
        }
    }
}