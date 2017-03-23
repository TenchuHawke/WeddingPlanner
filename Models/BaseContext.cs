using Microsoft.EntityFrameworkCore;
namespace WeddingPlanner.Models
{
    public class BaseContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public BaseContext(DbContextOptions<BaseContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Wedding> Weddings { get; set; }
        public DbSet<WeddingGuest> WeddingGuests { get; set; }
    }
}