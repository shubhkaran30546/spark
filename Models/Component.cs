using System.ComponentModel.DataAnnotations;

namespace spark.Models
{
    public class Component
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }

        // Foreign key for Computer
        public int ComputerId { get; set; }
        public Computer Computer { get; set; }
        public List<OrderComponent> OrderComponents { get; set; } = new List<OrderComponent>();
    }
}
