using MarketPlace.Api.DTOs.Common;
using MarketPlace.Api.DTOs.Products;

namespace MarketPlace.Api.Services.Interfaces;

public interface IProductService
{
    Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
    Task<ProductResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductResponse>> GetByStoreIdAsync(int storeId, CancellationToken cancellationToken = default);
    Task<PagedResponse<ProductResponse>> SearchAsync(ProductSearchQuery query, CancellationToken cancellationToken = default);
}
