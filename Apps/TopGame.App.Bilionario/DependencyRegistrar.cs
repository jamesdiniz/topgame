using Autofac;
using TopGame.App.Bilionario.Repository;
using TopGame.App.Bilionario.Service;
using TopGame.Core.Infrastructure;
using TopGame.Core.Infrastructure.DependencyManagement;

namespace TopGame.App.Bilionario
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<AppService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<AppRepository>().As<IAppRepository>().InstancePerLifetimeScope();
        }
    }
}