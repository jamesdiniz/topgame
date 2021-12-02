using System.Linq;
using Autofac;
using Autofac.Integration.Mvc;
using TopGame.Core.Data;
using TopGame.Core.Infrastructure;
using TopGame.Core.Infrastructure.DependencyManagement;
using TopGame.Core.Infrastructure.Services;
using TopGame.Data;
using TopGame.Service;

namespace TopGame.Web.Framework
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            // controlllers
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            // context and repository
            builder.Register<IDbContext>(c => new TopGameContext()).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            //services
            builder.RegisterType<ConfiguracaoService>().As<IConfiguracaoService>().InstancePerLifetimeScope();
            builder.RegisterType<PontuacaoService>().As<IPontuacaoService>().InstancePerLifetimeScope();
            builder.RegisterType<JogadorService>().As<IJogadorService>().InstancePerLifetimeScope();
            builder.RegisterType<JogoService>().As<IJogoService>().InstancePerLifetimeScope();
            builder.RegisterType<PerguntaService>().As<IPerguntaService>().InstancePerLifetimeScope();
        }
    }
}