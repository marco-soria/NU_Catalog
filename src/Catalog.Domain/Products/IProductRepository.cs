namespace Catalog.Domain.Products;

public interface IProductRepository
{
    Task<Product?> GetByCode(string code, CancellationToken cancellationToken);
    Task<List<Product>> GetAll(CancellationToken cancellationToken);
    void Add(Product product);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}