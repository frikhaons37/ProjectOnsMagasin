using Microsoft.EntityFrameworkCore;

namespace ProjectOnsMagasin;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<User>> GetAll()
    {
        return await _dbContext.Users.ToListAsync();
    }
    public async Task<User?> GetById(int id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
    public async Task<User?> GetByMail(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task Add(User category)
    {
        await _dbContext.Users.AddAsync(category);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<List<User>> GetMostLoyalUsers()
    {
        return await _dbContext.Users.Include(e => e.Orders.Where(e => e != null &&
                                                                e.OrderType == OrderTypeEnum.Invoice))
                              .ThenInclude(e => e.OrdersProducts)
                              .Where(e => e.Orders != null && 
                                          e.Orders.Any(e => e.OrderType == OrderTypeEnum.Invoice))
                              .OrderByDescending(e => e.Orders.Sum(e => e.TotalPrice))
                              .ToListAsync();
    }
}
