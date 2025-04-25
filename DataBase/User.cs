using Microsoft.AspNetCore.Identity;
namespace brunchie_backend.DataBase
{
    public class User:IdentityUser
    {

        public string FullName { get; set; } = "None";
    }
}
