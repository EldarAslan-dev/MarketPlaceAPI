namespace MarketPlace.Api.DTOs.Orders;

public sealed record OrderResponse(
    int Id,
    int BuyerId,
    int ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice,
    DateTimeOffset CreatedAt);
