namespace ProjectOnsMagasin;

public interface IOrderProductRepository
{
    Task Add(OrderProduct orderProducr); 
    Task Edit(OrderProduct orderProduct);
    Task Remove(int productId, int orderId);
    Task<OrderProduct?> GetById(int productId, int orderId);
    Task<List<OrderProduct>> GetOrderProductsByOrderId(int orderId);
}
