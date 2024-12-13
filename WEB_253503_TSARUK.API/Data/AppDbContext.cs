using Microsoft.EntityFrameworkCore;
using WEB_253503_TSARUK.Domain.Entities;

namespace WEB_253503_TSARUK.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Jewelry> Jewelries { get; set; }
    }
}
