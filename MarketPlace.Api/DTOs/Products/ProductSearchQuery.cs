using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Api.DTOs.Products;

public sealed class ProductSearchQuery
{
    public string? Search { get; init; }
    public int? CategoryId { get; init; }
    public int? StoreId { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }

    [Range(1, int.MaxValue)]
    public int Page { get; init; } = 1;

    [Range(1, 100)]
    public int PageSize { get; init; } = 20;
}
