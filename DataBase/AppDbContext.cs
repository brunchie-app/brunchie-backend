using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using brunchie_backend.Models;
namespace brunchie_backend.DataBase

{
    public class AppDbContext:IdentityDbContext<User>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<Campus> Campus { get; set; }

    }
}
