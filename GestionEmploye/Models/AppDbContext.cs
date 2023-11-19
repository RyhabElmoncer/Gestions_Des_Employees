using Microsoft.EntityFrameworkCore;

namespace GestionEmploye.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Employe> Employees { get; set; }
    }
}
