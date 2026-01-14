using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace spark.Dtos
{
    public class CreateOrderDto
    {
        /// <summary>
        /// Computer id selected by the user for the order.
        /// </summary>
        [Required]
        public int ComputerId { get; set; }

        /// <summary>
        /// List of selected component ids to include in the order.
        /// </summary>
        [Required]
        public List<int> ComponentIds { get; set; } = new();
    }
}
