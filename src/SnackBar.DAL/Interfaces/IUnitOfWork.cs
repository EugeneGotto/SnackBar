using SnackBar.DAL.Models;
using System;
using System.Data;

namespace SnackBar.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TModel> Repository<TModel>() where TModel : BaseModel, new();

        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);

        bool Commit();

        void Rollback();

        int Save();

        void Dispose(bool disposing);
    }
}