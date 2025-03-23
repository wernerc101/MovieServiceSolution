using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieService.Api.Interfaces;
using MovieService.Api.Models;

namespace MovieService.Api.Repositories
{
    public class MySqlRepository : IRepository<CachedEntry>
    {
        private readonly DbContext _context;

        public MySqlRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<CachedEntry> GetByIdAsync(int id)
        {
            return await _context.Set<CachedEntry>().FindAsync(id);
        }

        public async Task<IEnumerable<CachedEntry>> GetAllAsync()
        {
            return await _context.Set<CachedEntry>().ToListAsync();
        }

        public async Task AddAsync(CachedEntry entity)
        {
            await _context.Set<CachedEntry>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CachedEntry entity)
        {
            _context.Set<CachedEntry>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<CachedEntry>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}