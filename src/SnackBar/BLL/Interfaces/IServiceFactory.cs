namespace SnackBar.BLL.Interfaces
{
    public interface IServiceFactory
    {
        IAdminService GetAdminService();

        IOrderService GetOrderService();

        IPaymentService GetPaymentService();

        IProductService GetProductService();

        IUserService GetUserService();

        ICartService GetCartService();

        ITagService GetTagService();
    }
}