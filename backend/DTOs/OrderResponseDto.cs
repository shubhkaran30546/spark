// Models/DTOs/OrderResponseDto.cs
namespace spark.Dtos
{
    /// <summary>
    /// DTO returned to clients describing an order and its selected items.
    /// </summary>
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public ComputerDto Computer { get; set; }
        public List<ComponentDto> Components { get; set; } = new();
    }

    /// <summary>
    /// DTO representing a computer within an order response.
    /// </summary>
    public class ComputerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }

    /// <summary>
    /// DTO representing a component within an order response.
    /// </summary>
    public class ComponentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
    }
}
