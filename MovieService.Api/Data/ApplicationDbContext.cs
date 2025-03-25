using Microsoft.EntityFrameworkCore;

namespace MovieService.Api.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CachedEntry> CachedEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure the CachedEntry entity
            modelBuilder.Entity<CachedEntry>()
                .HasKey(e => e.Id);
            
            // Add any additional configurations here
        }
    }
}