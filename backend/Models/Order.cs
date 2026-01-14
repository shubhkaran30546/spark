using Microsoft.AspNetCore.Identity;

namespace spark.Models
{
    /// <summary>
    /// Represents a customer's order, including selected computer and components.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Order identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The identity user id who placed the order.
        /// </summary>
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Selected computer id and reference.
        /// </summary>
        public int ComputerId { get; set; }
        public Computer Computer { get; set; }

        /// <summary>
        /// Components included in this order.
        /// </summary>
        public List<OrderComponent> OrderComponents { get; set; } = new();

        /// <summary>
        /// Total price of the order calculated on the server.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// UTC timestamp when the order was placed.
        /// </summary>
        public DateTime OrderDate { get; set; }
    }
}
