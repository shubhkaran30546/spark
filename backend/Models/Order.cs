using Microsoft.AspNetCore.Identity;

namespace spark.Models
{
    public class Order
    {
        public int Id { get; set; }

        // Identity user
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ComputerId { get; set; }
        public Computer Computer { get; set; }

        public List<OrderComponent> OrderComponents { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
