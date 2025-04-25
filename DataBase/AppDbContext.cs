using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
namespace brunchie_backend.DataBase

{
    public class AppDbContext:IdentityDbContext<User>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

    }
}
