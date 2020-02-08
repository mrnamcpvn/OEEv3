using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OEE_API.Models.SHW_SHD;
using OEE_API.Data.Interfaces;

namespace OEE_API.Data.Repository
{
    //Thực thi các class đã định nghĩa trong IRepository
    public class RepositorySHW_SHD<T, K> : IRepositorySHW_SHD<T, K> where T : class
    {
        public readonly DBContextSHW_SHD _context;
        public RepositorySHW_SHD(DBContextSHW_SHD context)
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