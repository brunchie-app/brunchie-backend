using Microsoft.AspNetCore.Identity;
namespace brunchie_backend.DataBase
{
    public class User : IdentityUser
    {
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CampusId { get; set; } = "None";
    }
}
