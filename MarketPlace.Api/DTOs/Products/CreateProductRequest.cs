using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Api.DTOs.Products;

public sealed class CreateProductRequest
{
    [Range(1, int.MaxValue)]
    public int SellerId { get; init; }

    [Range(1, int.MaxValue)]
    public int StoreId { get; init; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; init; }

    [Required, StringLength(200, MinimumLength = 2)]
    public required string Name { get; init; }

    [Range(typeof(decimal), "0.01", "1000000")]
    public decimal Price { get; init; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; init; }
}
