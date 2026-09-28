using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Data;
using ProductManagementApi.Models;

namespace ProductManagementApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        // Private field to access the EF Core DbContext
        private readonly ApplicationDbContext _context;

        // Logger instance used to log repository-level activities
        private readonly ILogger<ProductRepository> _logger;

        // Constructor injection to receive the ApplicationDbContext instance
        public ProductRepository(ApplicationDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Returns all products from the database in descending order of Id
        public async Task<List<Product>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all products from the database.");

                return await _context.Products
                    .AsNoTracking() // Improves performance for read-only operations
                    .OrderByDescending(p => p.Id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all products from the database.");
                throw;
            }
        }

        // Returns a single product matching the specified Id
        public async Task<Product?> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching product with Id {ProductId} from the database.", id);

                return await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching product with Id {ProductId}.", id);
                throw;
            }
        }

        // Adds a new product to the DbContext
        public async Task AddAsync(Product product)
        {
            try
            {
                _logger.LogInformation("Adding product named {ProductName} to the DbContext.", product.Name);

                await _context.Products.AddAsync(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding product named {ProductName}.", product.Name);
                throw;
            }
        }

        // Marks the given product as updated in the DbContext
        public void Update(Product product)
        {
            try
            {
                _logger.LogInformation("Updating product with Id {ProductId} in the DbContext.", product.Id);

                _context.Products.Update(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product with Id {ProductId}.", product.Id);
                throw;
            }
        }

        // Marks the given product for deletion from the DbContext
        public void Delete(Product product)
        {
            try
            {
                _logger.LogInformation("Deleting product with Id {ProductId} from the DbContext.", product.Id);

                _context.Products.Remove(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product with Id {ProductId}.", product.Id);
                throw;
            }
        }

        // Saves all inserted, updated, or deleted changes to the database
        public async Task SaveAsync()
        {
            try
            {
                _logger.LogInformation("Saving changes to the database.");

                await _context.SaveChangesAsync();

                _logger.LogInformation("Database changes saved successfully.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred while saving changes.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while saving changes to the database.");
                throw;
            }
        }
    }
}
