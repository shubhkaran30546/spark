using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace spark.Models
{
    public class CreateOrderDto
    {
        [Required]
        public int ComputerId { get; set; }

        [Required]
        public List<int> ComponentIds { get; set; } = new();
    }
}
