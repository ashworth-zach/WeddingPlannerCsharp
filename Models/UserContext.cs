using Microsoft.EntityFrameworkCore;
 
namespace weddingplanner.Models
{
    public class UserContext : DbContext
    {
        public UserContext (DbContextOptions<UserContext> options) : base(options) {}
        public DbSet<User> user { get; set; }
        public DbSet<Wedding> wedding { get; set; }
        public DbSet<Guests> guests{get;set;}
    }
}