using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OEE_API.Models.SYF;
using OEE_API.Data.Interfaces;

namespace OEE_API.Data.Repository
{
  public class RepositorySYF<T, K> : IRepositorySYF<T, K> where T : class
    {
        public readonly DBContextSYF _context;
        public RepositorySYF(DBContextSYF context)
        {
            _context = context;
        }
        public IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items;
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items.Where(predicate);
        }

        public async Task<T> FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return await items.SingleOrDefaultAsync(predicate);
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void RemoveMultiple(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}