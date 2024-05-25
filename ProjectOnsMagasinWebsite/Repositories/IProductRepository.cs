namespace ProjectOnsMagasin;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAll();
    Task<Product?> GetById(int id);
    Task Add(Product product);
    Task Edit(Product product);
    Task Remove(int product);
    Task<IEnumerable<Product>> Filter(string? name = null, string? brand = null, int categoryId = 0);
    Task<IEnumerable<Product>> GetTheBestSellingItem();
}
