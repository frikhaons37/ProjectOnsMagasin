using Microsoft.EntityFrameworkCore;

namespace ProjectOnsMagasin;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrderRepository(ApplicationDbContext context)
    {
        _dbContext = context;
    }
    public async Task<Order?> GetById(int id)
    {
        return await _dbContext.Orders.AsNoTracking().Include(e => e.OrdersProducts).FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<Order?> GetByIdWithProducts(int id)
    {
        return await _dbContext.Orders.AsNoTracking()
                                      .Include(e => e.OrdersProducts)
                                      .ThenInclude(e=>e.Product)
                                      .FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<IEnumerable<Order>> GetAllOrders(int userId)
    {
        return await _dbContext.Orders.Where(e=>e.OrderType == OrderTypeEnum.Invoice && e.UserId == userId)
                                      .Include(e => e.OrdersProducts)
                                      .ThenInclude(e => e.Product)
                                      .ToListAsync();
    }
    public async Task<IEnumerable<Order>> GetAllOrders()
    {
        return await _dbContext.Orders.Where(e => e.OrderType == OrderTypeEnum.Invoice)
                                      .Include(e => e.OrdersProducts)
                                      .ThenInclude(e => e.Product)
                                      .ToListAsync();
    }
    public async Task<Order?> GetUserCartWithoutProducts(int userId)
    {
        Order? order = await _dbContext.Orders.Where(o => o.UserId == userId && o.OrderType == OrderTypeEnum.Cart)
                                            .Include(e => e.OrdersProducts)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync();

        return order;
    }
    public async Task<Order?> GetUserCartWithProducts(int userId)
    {
        Order? order = await _dbContext.Orders.Where(o => o.UserId == userId && o.OrderType == OrderTypeEnum.Cart)
                                            .Include(e => e.OrdersProducts)
                                            .ThenInclude(e=>e.Product)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync();

        return order;
    }

    public async Task Add(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Edit(Order order)
    {
        Order? orderFromDb = await GetById(order.Id);
        if (orderFromDb == null)
            throw new ArgumentNullException(nameof(order));

        await Task.Run(() => _dbContext.Orders.Update(order));
        await _dbContext.SaveChangesAsync();
    }
    public async Task Remove(int id)
    {
        Order? orderFromDb = await GetById(id);
        if (orderFromDb == null)
            throw new ArgumentNullException(nameof(orderFromDb));

        await Task.Run(() => _dbContext.Orders.Remove(orderFromDb));
        await _dbContext.SaveChangesAsync();
    }
    public async Task<List<Order>> ListUnPaidOrders()
    {
        return await _dbContext.Orders.Where(e => e.OrderType == (int)OrderTypeEnum.Cart)
                                      .Include(e => e.OrdersProducts)
                                      .ToListAsync();  
    }
    public async Task<List<Order>> ListOrdersOfUser(int userId)
    {
        return await _dbContext.Orders.Where(e => e.OrderType == OrderTypeEnum.Invoice &&
                                                  e.UserId == userId)
                                      .Include(e => e.OrdersProducts)
                                      .ToListAsync();
    }

}
