// File: Models/Customer.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace spark.Models
{
    public class Customer
    {
        [Key]
        public string Id { get; set; } // Ensure it's a string

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public string SecurityQuestion { get; set; }
        public string SecurityAnswerHash { get; set; }
    }
}
