using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OEE_API.Data.Interfaces
{
    public interface IRepositorySHB<T, K> where T : class
    {
        Task<T> FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void RemoveMultiple(List<T> entities);
        Task<bool> SaveAll();
    }
}