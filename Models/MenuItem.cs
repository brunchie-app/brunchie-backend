using brunchie_backend.DataBase;
using System.ComponentModel.DataAnnotations;

namespace brunchie_backend.Models
{
    public class MenuItem
    {
        public int ItemId { get; set; }


        public string VendorId { get; set; } = null!;

        
        public string Name { get; set; } = null!;

        
        public string Description { get; set; } = null!;
        
        
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }

        
        public bool IsAvailable { get; set; }

        
    }

}
