using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieService.Api.Interfaces;

namespace MovieService.Api.Repositories
{
    public class MySqlRepository : IRepository<CachedEntry>
    {
        private readonly ApplicationDbContext _context;

        public MySqlRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CachedEntry> GetByIdAsync(int id)
        {
            return await _context.CachedEntries.FindAsync(id);
        }

        public async Task<IEnumerable<CachedEntry>> GetAllAsync()
        {
            return await _context.CachedEntries.ToListAsync();
        }

        public async Task AddAsync(CachedEntry entity)
        {
            await _context.CachedEntries.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CachedEntry entity)
        {
            _context.CachedEntries.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.CachedEntries.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}