using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SnackBar.App_Start
{
    public class CustomDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// Application kernel of DependencyResolver
        /// </summary>
        private readonly IKernel _kernel;

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        /// <param name="kernel">Kernel</param>
        public CustomDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        /// <summary>
        /// Resolves singly registered services that support arbitrary object creation
        /// </summary>
        /// <param name="serviceType">The type of the requested service or object</param>
        /// <returns>The requested service or object</returns>
        public object GetService(Type serviceType)
        {
            return _kernel.Get(serviceType);
        }

        /// <summary>
        /// Resolves multiply registered services
        /// </summary>
        /// <param name="serviceType">The type of the requested services</param>
        /// <returns>The requested services</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}