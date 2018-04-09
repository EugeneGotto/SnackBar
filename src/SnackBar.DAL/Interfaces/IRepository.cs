using SnackBar.DAL.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SnackBar.DAL.Interfaces
{
    public interface IRepository<TModel> : IDisposable where TModel : BaseModel, new()
    {
        TModel GetById(long id, params Expression<Func<TModel, BaseModel>>[] includes);

        IQueryable<TModel> Find(params Expression<Func<TModel, BaseModel>>[] includes);

        IQueryable<TModel> Find(Expression<Func<TModel, bool>> filter, params Expression<Func<TModel, BaseModel>>[] includes);

        TModel FirstOrDefault(Expression<Func<TModel, bool>> filter, params Expression<Func<TModel, BaseModel>>[] includes);

        int Count(Expression<Func<TModel, bool>> filter = null);

        bool Exist(Expression<Func<TModel, bool>> filter);

        TModel AddOrUpdate(TModel model);

        void AddOrUpdate(TModel[] models);

        void DeleteById(long id);

        void Delete(TModel model);

        void SoftDeleteById(long id);

        void SoftDelete(TModel model);
    }
}