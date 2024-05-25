using System.Text.Json.Serialization;

namespace ProjectOnsMagasin;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public double Price { get; set; }
    public int Qunatity { get; set; }
    public string ImagePath { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    [JsonIgnore]
    public ICollection<OrderProduct>? ordersProducts { get; set; }
}
