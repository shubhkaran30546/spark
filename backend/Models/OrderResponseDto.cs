// Models/DTOs/OrderResponseDto.cs
namespace spark.Models
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public ComputerDto Computer { get; set; }
        public List<ComponentDto> Components { get; set; } = new();
    }

    public class ComputerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class ComponentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
    }
}
