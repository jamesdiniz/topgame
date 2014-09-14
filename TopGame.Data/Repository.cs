using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using TopGame.Core.Data;

namespace TopGame.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Fields

        private readonly IDbContext _context;
        private IDbSet<T> _entities;

        #endregion

        public Repository(IDbContext context)
        {
            _context = context;
        }

        public virtual T GetById(object id)
        {
            return Entities.Find(id);
        }

        public virtual void Add(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                Entities.Add(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbException)
            {
                throw GetException(dbException);
            }
        }

        public virtual void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbException)
            {
                throw GetException(dbException);
            }
        }

        public virtual void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                Entities.Remove(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbException)
            {
                throw GetException(dbException);
            }
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return Entities;
            }
        }

        protected virtual IDbSet<T> Entities
        {
            get { return _entities ?? (_entities = _context.Set<T>()); }
        }

        private static Exception GetException(DbEntityValidationException dbException)
        {
            var msg = string.Empty;

            foreach (var validationErrors in dbException.EntityValidationErrors)
                foreach (var validationError in validationErrors.ValidationErrors)
                    msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

            return new Exception(msg, dbException);
        }
    }
}
