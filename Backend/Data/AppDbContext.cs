using Microsoft.EntityFrameworkCore;
using Backend.Models;  // Kontrollera att denna rad är rätt och att semikolonet är med!

namespace Backend.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
