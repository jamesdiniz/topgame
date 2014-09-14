using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace TopGame.Data
{
    public class TopGameContext : DbContext, IDbContext
    {
        public TopGameContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Database.Log = s => Debug.WriteLine(s);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !String.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            foreach (var configurationInstance in typesToRegister.Select(Activator.CreateInstance))
                modelBuilder.Configurations.Add((dynamic) configurationInstance);

            base.OnModelCreating(modelBuilder);
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public IList<TEntity> ExecuteStoredProcedure<TEntity>(string commandText, params object[] parameters) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return Database.SqlQuery<TElement>(sql, parameters);
        }

        public int ExecuteSqlCommand(string sql, int? timeout = null, params object[] parameters)
        {
            return Database.ExecuteSqlCommand(sql, parameters);
        }
    }
}