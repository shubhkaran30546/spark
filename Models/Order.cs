namespace spark.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int ComputerId { get; set; }
        public Computer Computer { get; set; }
        public List<OrderComponent> OrderComponents { get; set; } = new List<OrderComponent>();
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
