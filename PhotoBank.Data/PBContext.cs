using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhotoBank.Models;

namespace PhotoBank.Data
{
    public class PbContext: IdentityDbContext<User>
    {
        public PbContext(DbContextOptions<PbContext> options):base (options)
        {
           // Database.EnsureCreated();
        }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}
