using MarketPlace.Api.Data;
using MarketPlace.Api.DTOs.Common;
using MarketPlace.Api.DTOs.Products;
using MarketPlace.Api.Entities;
using MarketPlace.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Api.Repositories;

public sealed class ProductRepository(AppDbContext db) : IProductRepository
{
    public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        db.Products.AsNoTracking().Include(x => x.Store).Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Product>> GetByStoreIdAsync(int storeId, CancellationToken cancellationToken = default) =>
        await db.Products.AsNoTracking().Include(x => x.Store).Include(x => x.Category)
            .Where(x => x.StoreId == storeId).OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task<PagedResponse<Product>> SearchAsync(ProductSearchQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<Product> products = db.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            products = products.Where(x => x.Name.Contains(search));
        }

        if (query.CategoryId.HasValue)
            products = products.Where(x => x.CategoryId == query.CategoryId.Value);

        if (query.StoreId.HasValue)
            products = products.Where(x => x.StoreId == query.StoreId.Value);

        if (query.MinPrice.HasValue)
            products = products.Where(x => x.Price >= query.MinPrice.Value);

        if (query.MaxPrice.HasValue)
            products = products.Where(x => x.Price <= query.MaxPrice.Value);

        var total = await products.CountAsync(cancellationToken);
        var offset = ((long)query.Page - 1) * query.PageSize;
        if (offset > int.MaxValue)
            return new PagedResponse<Product>([], query.Page, query.PageSize, total);

        var items = await products.Include(x => x.Store).Include(x => x.Category)
            .OrderBy(x => x.Id).Skip((int)offset).Take(query.PageSize).ToListAsync(cancellationToken);

        return new PagedResponse<Product>(items, query.Page, query.PageSize, total);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default) =>
        await db.Products.AddAsync(product, cancellationToken);

    public async Task<bool> TryDecrementStockAsync(int productId, int quantity, CancellationToken cancellationToken = default)
    {
        var changed = await db.Products
            .Where(x => x.Id == productId && x.StockQuantity >= quantity)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.StockQuantity, x => x.StockQuantity - quantity), cancellationToken);

        return changed == 1;
    }
}
