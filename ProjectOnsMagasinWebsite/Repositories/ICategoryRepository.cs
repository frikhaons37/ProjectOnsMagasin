namespace ProjectOnsMagasin;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAll();
    Task<Category?> GetById(int id);
    Task Add(Category product);
    Task Edit(Category product);
    Task Remove(int id);
    IList<Category> Search(string name);
}
