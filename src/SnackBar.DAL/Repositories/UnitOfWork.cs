using SnackBar.DAL.Interfaces;
using SnackBar.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace SnackBar.DAL.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private IDataContext _dataContext;
        private ObjectContext _objectContext;
        private Dictionary<string, dynamic> _repositories;
        private DbTransaction _transaction;

        public UnitOfWork(IDataContext dataContext)
        {
            _dataContext = dataContext;
            _repositories = new Dictionary<string, dynamic>();
        }

        public IRepository<TModel> Repository<TModel>() where TModel : BaseModel, new()
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<string, dynamic>();
            }

            var type = typeof(TModel).Name;

            if (_repositories.ContainsKey(type))
            {
                return (IRepository<TModel>)_repositories[type];
            }

            var repositoryType = typeof(Repository<>);

            _repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TModel)), _dataContext, this));

            return _repositories[type];
        }

        public int Save()
        {
            return _dataContext.SaveChanges();
        }

        public void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_dataContext != null)
                {
                    _dataContext.Dispose();
                    _dataContext = null;
                }

                if (_repositories != null)
                {
                    _repositories = null;
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            _objectContext = ((IObjectContextAdapter)_dataContext).ObjectContext;
            if (_objectContext.Connection.State != ConnectionState.Open)
            {
                _objectContext.Connection.Open();
            }

            _transaction = _objectContext.Connection.BeginTransaction(isolationLevel);
        }

        public bool Commit()
        {
            _transaction.Commit();
            return true;
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _dataContext.SyncObjectsStatePostCommit();
        }
    }
}