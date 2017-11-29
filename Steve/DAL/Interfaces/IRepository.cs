using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Steve.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteById(object id);
        IQueryable<T> Set { get; }
        IQueryable<T> GetWithInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] include);
        IQueryable<T> Include(params Expression<Func<T, object>>[] include);
        void RemoveRange(IQueryable<T> entities);
    }
}
