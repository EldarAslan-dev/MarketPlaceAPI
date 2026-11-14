using MarketPlace.Api.DTOs.Common;
using MarketPlace.Api.DTOs.Products;
using MarketPlace.Api.Entities;

namespace MarketPlace.Api.Repositories.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetByStoreIdAsync(int storeId, CancellationToken cancellationToken = default);
    Task<PagedResponse<Product>> SearchAsync(ProductSearchQuery query, CancellationToken cancellationToken = default);
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    Task<bool> TryDecrementStockAsync(int productId, int quantity, CancellationToken cancellationToken = default);
}
