using System;

namespace SnackBar.DAL.Interfaces
{
    public interface IDataContext : IDisposable
    {
        int SaveChanges();

        void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState;

        void SyncObjectsStatePostCommit();
    }
}