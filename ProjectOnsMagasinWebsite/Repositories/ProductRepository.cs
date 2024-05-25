
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ProjectOnsMagasin;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        return await _dbContext.Products.Include(e => e.Category).ToListAsync();
    }
    public async Task<Product?> GetById(int id)
    {
        return  await _dbContext.Products
                                .Include(e=>e.Category)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task Add(Product product)
    {
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Edit(Product product)
    {
        Product? productFromDb = await GetById(product.Id);
        if (productFromDb == null)
            throw new ArgumentNullException(nameof(product));

        if (product.ImagePath == null)
            product.ImagePath = productFromDb.ImagePath;

        productFromDb = product; 

        await Task.Run(() => _dbContext.Products.Update(productFromDb));
        await _dbContext.SaveChangesAsync();
    }

    public async Task Remove(int id)
    {
        Product? productFromDb = await GetById(id);
        if (productFromDb == null) 
            throw new ArgumentNullException(nameof(productFromDb));

        await Task.Run(() => _dbContext.Products.Remove(productFromDb));
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> Filter(string? name = null, string? brand = null, int categoryId = 0)
    {
        IQueryable<Product> query = _dbContext.Products;

        if (!name.IsNullOrEmpty())
            query = query.Where(e => e.Name.Contains(name!));


        if (!brand.IsNullOrEmpty())
            query = query.Where(e => e.Brand.Contains(brand!));

        if (categoryId > 0)
            query = query.Where(e => e.CategoryId == categoryId);

        return await query.Include(e => e.Category).ToListAsync();

    }
    public async Task<IEnumerable<Product>> GetTheBestSellingItem()
    {
        return await _dbContext.Products.Include(e => e.Category)
                                        .Include(e => e.ordersProducts
                                        .Where(e => e.Order.OrderType == OrderTypeEnum.Invoice))
                                        .OrderByDescending(e => e.ordersProducts.Sum(e => e.Quantity))
                                        .ToListAsync();
    }
}

