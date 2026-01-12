namespace spark.Models
{

    public class OrderComponent
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int ComponentId { get; set; }
    public Component Component { get; set; } = null!;
}

}