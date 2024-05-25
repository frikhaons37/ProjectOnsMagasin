using Microsoft.EntityFrameworkCore;

namespace ProjectOnsMagasin;

public class OrderProductRepository : IOrderProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrderProductRepository(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public async Task Add(OrderProduct orderProducr)
    {
        await _dbContext.OrderProducts.AddAsync(orderProducr);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<OrderProduct?> GetById(int productId, int orderId)
    {
        return await _dbContext.OrderProducts.AsNoTracking().FirstOrDefaultAsync(p => p.OrderId == orderId &&
                                                                                      p.ProductId == productId);
    }
    public async Task Remove(int productId, int orderId)
    {
        OrderProduct? orderProductFromDb = await GetById(productId, orderId);
        if (orderProductFromDb == null)
            throw new ArgumentNullException(nameof(orderProductFromDb));

        await Task.Run(() => _dbContext.OrderProducts.Remove(orderProductFromDb));
        await _dbContext.SaveChangesAsync();
    }
    public async Task Edit(OrderProduct orderProducr)
    {
        await Task.Run(() => _dbContext.OrderProducts.Update(orderProducr));
        await _dbContext.SaveChangesAsync();
    }
    public async Task<List<OrderProduct>> GetOrderProductsByOrderId(int orderId)
    {
        return await _dbContext.OrderProducts.Where(e=>e.OrderId == orderId)
                                             .Include(e=>e.Product)
                                             .ToListAsync(); 
    }
}