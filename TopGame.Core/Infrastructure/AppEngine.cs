using System;
using System.Linq;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using TopGame.Core.Infrastructure.DependencyManagement;

namespace TopGame.Core.Infrastructure
{
    public class AppEngine : IAppEngine
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Container manager
        /// </summary>
        public ContainerManager ContainerManager { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// Inicializa mecanismo
        /// </summary>
        public void Initialize()
        {
            RegisterDependencies();
        }

        /// <summary>
        /// Resolve dependência
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        /// <summary>
        /// Resolve dependência
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        /// <summary>
        /// Resolve dependência
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Registra todas as depências
        /// </summary>
        protected void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();

            // Registra dependências desse assembly
            var typeFinder = new AppTypeFinder();
            builder = new ContainerBuilder();
            builder.RegisterInstance(this).As<IAppEngine>().SingleInstance();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            builder.Update(container);

            // Registra dependências de outros assemblies
            builder = new ContainerBuilder();
            var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();

            if (drTypes != null)
            {
                var drInstances = drTypes.Select(drType => (IDependencyRegistrar)Activator.CreateInstance(drType)).ToList();
                drInstances.ForEach(registrar => registrar.Register(builder, typeFinder));

                builder.Update(container);
            }

            // Armazena instância do container responsável por resolver dependências
            ContainerManager = new ContainerManager(container);

            // Configura dependências
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        #endregion
    }
}