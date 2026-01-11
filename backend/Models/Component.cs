using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace spark.Models
{
    public class Component
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;   // "16GB RAM"
        public decimal Price { get; set; }
        public string Type { get; set; } = string.Empty; // RAM, Storage, GPU

        public int ComputerId { get; set; }
        [JsonIgnore]
        public Computer Computer { get; set; }

        public ICollection<OrderComponent> OrderComponents { get; set; } = new List<OrderComponent>();
    }
}


