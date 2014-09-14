using System.Linq;

namespace TopGame.Core.Data
{
    public interface IRepository<T> where T : class
    {
        T GetById(object id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        IQueryable<T> Table { get; }
    }
}
