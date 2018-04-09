namespace SnackBar.DAL.Interfaces
{
    public interface IDalFactory
    {
        IUnitOfWork GetUnitOfWork();
    }
}