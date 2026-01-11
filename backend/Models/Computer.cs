using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace spark.Models
{
    /// <summary>
    /// Represents a computer product with pricing, description, and components.
    /// </summary>
    public class Computer
    {
        /// <summary>
        /// Unique identifier for the computer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the computer.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Price of the computer.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Detailed description of the computer.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// URL of the computer image.
        /// </summary>
        public string ImageUrl { get; set; }

        public ICollection<OrderComponent> OrderComponents { get; set; } = new List<OrderComponent>();
        public ICollection<Component> Components { get; set; } = new List<Component>();
    }
}
