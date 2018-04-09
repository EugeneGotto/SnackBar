using NLog;
using SnackBar.DAL.Interfaces;
using System;
using System.Data.Entity.Validation;

namespace SnackBar.BLL.Services
{
    public abstract class BaseService
    {
        private Logger _logger = LogManager.GetLogger("Cart");

        protected readonly IDalFactory _dalfactory;

        public BaseService(IDalFactory factory)
        {
            _dalfactory = factory;
        }

        protected virtual TResult InvokeInUnitOfWorkScope<TResult>(Func<IUnitOfWork, TResult> func)
        {
            var result = default(TResult);

            this.TryInvokeServiceActionInUnitOfWorkScope(
                work =>
                {
                    result = func.Invoke(work);
                });

            return result;
        }

        protected virtual void InvokeInUnitOfWorkScope(Action<IUnitOfWork> action)
        {
            this.TryInvokeServiceActionInUnitOfWorkScope(action.Invoke);
        }

        private void TryInvokeServiceActionInUnitOfWorkScope(Action<IUnitOfWork> action)
        {
            this.TryInvokeServiceAction(
                () =>
                {
                    using (var unitOfWork = this._dalfactory.GetUnitOfWork())
                    {
                        action.Invoke(unitOfWork);
                    }
                });
        }

        private void TryInvokeServiceAction(Action action)
        {
            try
            {
                action.Invoke();
            }
            /*catch (InvalidOperationException exception)
            {
            }   */
            catch (DbEntityValidationException ex)
            {
                _logger.Warn($"{ex.Message}");
                throw ex;
            }
            catch (Exception exception)
            {
                _logger.Warn($"{exception.Message}");
                throw new InvalidOperationException("cannot invoke service action", exception);
            }
        }
    }
}