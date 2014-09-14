using System.Collections.Generic;
using System.Data.Entity;

namespace TopGame.Data
{
    public interface IDbContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();

        IList<TEntity> ExecuteStoredProcedure<TEntity>(string commandText, params object[] parameters)
            where TEntity : class, new();

        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);

        int ExecuteSqlCommand(string sql, int? timeout = null, params object[] parameters);
    }
}
