using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;

namespace TopGame.Core.Infrastructure.DependencyManagement
{
    public class ContainerManager
    {
        #region Fields

        private readonly IContainer _container;
        //private readonly ScopeProvider _scopeProvider;

        #endregion

        #region Properties

        #endregion

        #region Ctor

        public ContainerManager(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            _container = container;
        }

        #endregion

        #region Methods

        public T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class
        {
            if (scope == null)
            {
                scope = GetScope();
            }
            return string.IsNullOrEmpty(key) 
                ? scope.Resolve<T>() 
                : scope.ResolveKeyed<T>(key);
        }

        public object Resolve(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = GetScope();
            }
            return scope.Resolve(type);
        }

        public T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = GetScope();
            }
            return string.IsNullOrEmpty(key) 
                ? scope.Resolve<IEnumerable<T>>().ToArray() 
                : scope.ResolveKeyed<IEnumerable<T>>(key).ToArray();
        }

        #endregion

        #region Utilities

        public ILifetimeScope GetScope()
        {
            var scopeProvider = new ScopeProvider(_container);
            return scopeProvider.GetLifetimeScope(null);
        }

        #endregion
    }
}