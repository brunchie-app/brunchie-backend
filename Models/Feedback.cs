namespace brunchie_backend.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int OrderId { get; set; }
        public string VendorId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        
    }


}
