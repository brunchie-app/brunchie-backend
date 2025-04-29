using brunchie_backend.DataBase;

namespace brunchie_backend.Models
{
    public class Order
    {
        public int OrderId { get; set; }  

        public string StudentId { get; set; }  
        

        public string VendorId { get; set; }  
        
        public string OrderStatus { get; set; }  

        public decimal TotalAmount { get; set; }  

        public DateTime CreatedAt { get; set; }  
    }

}
