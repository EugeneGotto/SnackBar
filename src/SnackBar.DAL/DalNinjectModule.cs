using Ninject.Extensions.Factory;
using Ninject.Modules;
using SnackBar.DAL.Context;
using SnackBar.DAL.Interfaces;
using SnackBar.DAL.Repositories;
using System.Data.Entity;

namespace SnackBar.DAL
{
    public class DalNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<DbContext>().To<BarContext>();
            this.Bind<IDataContext>().To<BarContext>();

            this.Bind(typeof(IRepository<>)).To(typeof(Repository<>));
            this.Bind<IUnitOfWork>().To<UnitOfWork>().NamedLikeFactoryMethod((IDalFactory f) => f.GetUnitOfWork());

            Bind<IDalFactory>().ToFactory();
        }
    }
}