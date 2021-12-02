using System;
using System.Web;
using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;

namespace TopGame.Core.Infrastructure.DependencyManagement
{
    /// <summary>
    /// Provedor de acesso ao escopo durante requisição
    /// </summary>
    public class ScopeProvider : ILifetimeScopeProvider
    {
        private readonly IContainer _container;
        private ILifetimeScope _scope;

        public ScopeProvider(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            _container = container;
        }

        public ILifetimeScope ApplicationContainer
        {
            get { return _container; }
        }

        public void EndLifetimeScope()
        {
            if (_scope == null) return;

            _scope.Dispose();
            _scope = null;
        }

        public ILifetimeScope GetLifetimeScope(Action<ContainerBuilder> configurationAction)
        {
            if (_scope == null)
            {
                if (HttpContext.Current != null)
                {
                    _scope = AutofacDependencyResolver.Current.RequestLifetimeScope;
                }
                else
                {
                    _scope = (configurationAction == null)
                        ? ApplicationContainer.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag)
                        : ApplicationContainer.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag, configurationAction);
                }
            }
            return _scope;
        }
    }
}