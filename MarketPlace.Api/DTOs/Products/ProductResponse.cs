namespace MarketPlace.Api.DTOs.Products;

public sealed record ProductResponse(
    int Id,
    string Name,
    decimal Price,
    int StockQuantity,
    int StoreId,
    string StoreName,
    int CategoryId,
    string CategoryName,
    DateTimeOffset CreatedAt);
