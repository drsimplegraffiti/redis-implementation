
using Microsoft.EntityFrameworkCore;

namespace CachingRedis.Models.Data;
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
            
        {
            
        }

        public DbSet<Driver> Drivers { get; set; }
    }