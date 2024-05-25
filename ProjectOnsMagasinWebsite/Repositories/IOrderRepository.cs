namespace ProjectOnsMagasin;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllOrders();
    Task<IEnumerable<Order>> GetAllOrders(int userId);
    Task<Order?> GetUserCartWithoutProducts(int userId);
    Task<Order?> GetUserCartWithProducts(int userId);
    Task Add(Order order);
    Task Edit(Order order);
    Task<Order?> GetById(int id);
    Task Remove(int id);
    Task<List<Order>> ListUnPaidOrders();
    Task<List<Order>> ListOrdersOfUser(int userId);
    Task<Order?> GetByIdWithProducts(int id);

}
