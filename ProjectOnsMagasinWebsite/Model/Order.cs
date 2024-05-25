using System.Text.Json.Serialization;

namespace ProjectOnsMagasin;

public class Order
{
    public int Id { get; set; }
    public double TotalPrice {  get; set; }
    public OrderTypeEnum OrderType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public ICollection<OrderProduct> OrdersProducts { get; set; } = null!;

}