using ProductManagementApi.Models;
namespace ProductManagementApi.Repositories
{
    public interface IProductRepository
    {
        // Returns all products from the database
        Task<List<Product>> GetAllAsync();

        // Returns a single product based on the given Id
        Task<Product?> GetByIdAsync(int id);

        // Adds a new product to the DbContext
        Task AddAsync(Product product);

        // Marks an existing product as modified
        void Update(Product product);

        // Marks a product for deletion
        void Delete(Product product);

        // Saves all pending changes to the database
        Task SaveAsync();
    }
}
