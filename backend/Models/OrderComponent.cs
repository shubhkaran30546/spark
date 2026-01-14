namespace spark.Models
{

    /// <summary>
    /// Join entity linking an <see cref="Order"/> with a <see cref="Component"/>.
    /// </summary>
    public class OrderComponent
{
    /// <summary>
    /// Parent order id.
    /// </summary>
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    /// <summary>
    /// Component id included in the order.
    /// </summary>
    public int ComponentId { get; set; }
    public Component Component { get; set; } = null!;
}

}