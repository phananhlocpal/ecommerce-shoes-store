using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.EntitiesRepo
{
    public class GenericRepo<T> : IRepo<T> where T : class
    {
        protected readonly EcommerceDBContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepo(EcommerceDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> AddAsync(T item)
        {
            var newItem = await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();
            return newItem.Entity;
        }

        public async Task<bool> UpdateAsync(T item)
        {
            _dbSet.Update(item);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _dbSet.FindAsync(id);
            if (item == null) return false;

            _dbSet.Remove(item);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
