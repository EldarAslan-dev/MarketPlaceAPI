using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Api.DTOs.Orders;

public sealed class CreateOrderRequest
{
    [Range(1, int.MaxValue)]
    public int BuyerId { get; init; }

    [Range(1, int.MaxValue)]
    public int ProductId { get; init; }

    [Range(1, 10000)]
    public int Quantity { get; init; }
}
