using SnackBar.DAL.Interfaces;
using SnackBar.DAL.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;

namespace SnackBar.DAL.Repositories
{
    public class Repository<TModel> : IRepository<TModel> where TModel : BaseModel, new()
    {
        private readonly DbContext _dbContext;

        private readonly IDbSet<TModel> _currentDbSet;

        private readonly IUnitOfWork _unitOfWork;

        private bool _disposed;

        public Repository(DbContext dbContext, IUnitOfWork unitOfWork)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this._unitOfWork = unitOfWork;
            this._currentDbSet = this._dbContext.Set<TModel>();
        }

        protected DbContext DbContext
        {
            get { return this._dbContext; }
        }

        protected IDbSet<TModel> CurrentDbSet
        {
            get { return this._currentDbSet; }
        }

        public virtual TModel GetById(long id, params Expression<Func<TModel, BaseModel>>[] includes)
        {
            return this.FirstOrDefault(x => x.Id == id, includes);
        }

        public virtual IQueryable<TModel> Find(params Expression<Func<TModel, BaseModel>>[] includes)
        {
            IQueryable<TModel> query = this.LoadIncludes(_currentDbSet, includes);

            return query;
        }

        public virtual IQueryable<TModel> Find(Expression<Func<TModel, bool>> filter, params Expression<Func<TModel, BaseModel>>[] includes)
        {
            IQueryable<TModel> query = this.LoadIncludes(_currentDbSet.Where(filter), includes);
            return query;
        }

        public TModel FirstOrDefault(Expression<Func<TModel, bool>> filter, params Expression<Func<TModel, BaseModel>>[] includes)
        {
            if (includes == null || includes.Length == 0)
            {
                return this._currentDbSet.FirstOrDefault(filter);
            }

            IQueryable<TModel> query = this.LoadIncludes(_currentDbSet.Where(filter), includes);
            return query.FirstOrDefault();
        }

        public int Count(Expression<Func<TModel, bool>> filter = null)
        {
            if (filter == null)
            {
                return this._currentDbSet.Count(x => !x.IsDeleted);
            }

            return this._currentDbSet.Where(x => !x.IsDeleted).Count(filter);
        }

        public bool Exist(Expression<Func<TModel, bool>> filter)
        {
            return this._currentDbSet.Any(filter);
        }

        public virtual TModel AddOrUpdate(TModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            // if the model exists in Db then we have to update it
            var originalModel = this.GetById(model.Id);
            if (originalModel != null)
            {
                this.Update(originalModel, model);
                return model;
            }
            else
            {
                return this.Add(model);
            }
        }

        public virtual void DeleteById(long id)
        {
            var model = this.GetById(id);
            if (model != null)
            {
                this._currentDbSet.Remove(model);
            }
        }

        public virtual void Delete(TModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            this.DeleteById(model.Id);
        }

        public void SoftDeleteById(long id)
        {
            this.MarkAsDeleted(id);
        }

        public void SoftDelete(TModel model)
        {
            this.MarkAsDeleted(model);
        }

        public virtual void AddOrUpdate(TModel[] models)
        {
            this._dbContext.Set<TModel>().AddOrUpdate(models);
        }

        public IRepository<T> GetRepository<T>() where T : BaseModel, new()
        {
            return _unitOfWork.Repository<T>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    this._dbContext?.Dispose();
                    this._disposed = true;
                }
            }
        }

        protected virtual void Update(TModel modelFromDb, TModel model)
        {
            var entry = _dbContext.Entry(modelFromDb);
            entry.CurrentValues.SetValues(model);
        }

        protected virtual TModel Add(TModel model)
        {
            var returnResult = this._currentDbSet.Add(model);
            return returnResult;
        }

        protected IQueryable<TModel> LoadIncludes(IQueryable<TModel> queryResult, params Expression<Func<TModel, BaseModel>>[] includes)
        {
            foreach (var include in includes)
            {
                queryResult = queryResult.Include(include);
            }

            return queryResult;
        }

        protected virtual void MarkAsDeleted(long id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                this._dbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        protected virtual void MarkAsDeleted(TModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            this.MarkAsDeleted(model.Id);
        }
    }
}