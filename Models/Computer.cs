using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace spark.Models
{
    public class Computer
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
        // Navigation property for related components
        public List<Component> Components { get; set; } = new List<Component>();
    }
}
