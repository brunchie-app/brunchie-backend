using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace brunchie_backend.Models
{
    public class LoginDto
    {
        public string UserName { get; set; } = "None";
        public string Password { get; set; } = "None";
    }

    public class SignUpDto
    {
        [Required]
        public string UserName { get; set; } = "None";

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string CampusId { get; set; }

        public DateTime CreatedAt { get; set; }

    }

    public class OrderDto
    {


        [Required]
        public string StudentId { get; set; }

        [Required]
        public string VendorId { get; set; }

        [Required]

        public decimal TotalAmount { get; set; }

        [Required]

        public List<OrderItemDto> OrderItems { get; set; }
    }
    public class OrderItemDto
    {
        [Required]
        public int MenuItemId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal PriceAtOrder { get; set; }

    }

    public class OrderResponseDto { 
    
      public int OrderId { get; set; }
      public DateTime CreatedAt { get; set; }
    
    }



    public class OrderItemResponseDto
    {
        public string MenuItemName { get; set; }
        public string MenuItemDescription { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtOrder { get; set; }
        
        
    }


}


