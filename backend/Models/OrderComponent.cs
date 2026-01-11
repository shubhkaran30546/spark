namespace spark.Models
{

    public class OrderComponent
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } // Optional: For navigation purposes, if needed
        public int ComponentId { get; set; }
        public Component Component { get; set; } // Optional: For navigation purposes, if needed
    }
}