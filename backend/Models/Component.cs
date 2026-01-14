using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace spark.Models
{
    public class Component
    {
        /// <summary>
        /// Component identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Display name of the component (e.g. "16GB RAM").
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;   // "16GB RAM"

        /// <summary>
        /// Unit price for the component.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Component type/category (e.g. RAM, Storage, GPU).
        /// </summary>
        public string Type { get; set; } = string.Empty; // RAM, Storage, GPU

        /// <summary>
        /// Parent computer id.
        /// </summary>
        public int ComputerId { get; set; }

        /// <summary>
        /// Reference to the parent computer (ignored in JSON to avoid cycles).
        /// </summary>
        [JsonIgnore]
        public Computer Computer { get; set; }

        /// <summary>
        /// Orders that include this component.
        /// </summary>
        public ICollection<OrderComponent> OrderComponents { get; set; } = new List<OrderComponent>();
    }
}


