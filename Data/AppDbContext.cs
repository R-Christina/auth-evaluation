using auth.Models;
using Microsoft.EntityFrameworkCore;

namespace auth.Data.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Emp> Emp { get; set; }
        public DbSet<Role> Role { get; set; }
    }
}