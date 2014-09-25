using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using TopGame.Core.Data;
using TopGame.Core.Infrastructure;
using TopGame.Data;
using TopGame.Service;

namespace TopGame.Web.Framework
{
    public static class DependencyRegistrar
    {
        public static void Register(ContainerBuilder builder, Assembly[] assemblies)
        {
            builder.RegisterControllers(assemblies);
            builder.Register<IDbContext>(c => new TopGameContext()).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            //services
            builder.RegisterType<ConfiguracaoService>().As<IConfiguracaoService>().InstancePerLifetimeScope();
            builder.RegisterType<PontuacaoService>().As<IPontuacaoService>().InstancePerLifetimeScope();
            builder.RegisterType<JogadorService>().As<IJogadorService>().InstancePerLifetimeScope();
            builder.RegisterType<JogoService>().As<IJogoService>().InstancePerLifetimeScope();
            builder.RegisterType<PerguntaService>().As<IPerguntaService>().InstancePerLifetimeScope();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}