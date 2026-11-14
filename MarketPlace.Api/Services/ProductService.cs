using MarketPlace.Api.Data;
using MarketPlace.Api.DTOs.Common;
using MarketPlace.Api.DTOs.Products;
using MarketPlace.Api.Entities;
using MarketPlace.Api.Exceptions;
using MarketPlace.Api.Repositories.Interfaces;
using MarketPlace.Api.Services.Interfaces;

namespace MarketPlace.Api.Services;

public sealed class ProductService(
    IProductRepository products,
    IStoreRepository stores,
    ICategoryRepository categories,
    IUnitOfWork unitOfWork) : IProductService
{
    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var store = await stores.GetByIdAsync(request.StoreId, cancellationToken)
            ?? throw new ApiException(404, "store_not_found", "Store was not found.");

        if (store.OwnerId != request.SellerId)
            throw new ApiException(403, "not_store_owner", "Seller does not own this store.");

        var category = await categories.GetByIdAsync(request.CategoryId, cancellationToken)
            ?? throw new ApiException(404, "category_not_found", "Category was not found.");

        var name = request.Name.Trim();
        if (name.Length < 2)
            throw new ApiException(400, "invalid_name", "Product name must contain at least 2 characters.");

        var product = new Product
        {
            Name = name,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            StoreId = store.Id,
            CategoryId = category.Id
        };

        await products.AddAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new ProductResponse(product.Id, product.Name, product.Price, product.StockQuantity,
            store.Id, store.Name, category.Id, category.Name, product.CreatedAt);
    }

    public async Task<ProductResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await products.GetByIdAsync(id, cancellationToken)
            ?? throw new ApiException(404, "product_not_found", "Product was not found.");
        return Map(product);
    }

    public async Task<IReadOnlyList<ProductResponse>> GetByStoreIdAsync(int storeId, CancellationToken cancellationToken = default)
    {
        if (await stores.GetByIdAsync(storeId, cancellationToken) is null)
            throw new ApiException(404, "store_not_found", "Store was not found.");

        return (await products.GetByStoreIdAsync(storeId, cancellationToken)).Select(Map).ToList();
    }

    public async Task<PagedResponse<ProductResponse>> SearchAsync(ProductSearchQuery query, CancellationToken cancellationToken = default)
    {
        if (query.MinPrice < 0 || query.MaxPrice < 0 ||
            (query.MinPrice.HasValue && query.MaxPrice.HasValue && query.MinPrice > query.MaxPrice))
            throw new ApiException(400, "invalid_price_range", "Price range is invalid.");

        if (query.CategoryId <= 0 || query.StoreId <= 0)
            throw new ApiException(400, "invalid_filter", "CategoryId and StoreId must be positive.");

        var result = await products.SearchAsync(query, cancellationToken);
        return new PagedResponse<ProductResponse>(result.Items.Select(Map).ToList(), result.Page, result.PageSize, result.TotalCount);
    }

    private static ProductResponse Map(Product product) =>
        new(product.Id, product.Name, product.Price, product.StockQuantity,
            product.StoreId, product.Store.Name, product.CategoryId, product.Category.Name, product.CreatedAt);
}
