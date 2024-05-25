namespace ProjectOnsMagasin;

public class OrderProduct
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int Quantity { get; set; }
    public double Price { get; set; }
}