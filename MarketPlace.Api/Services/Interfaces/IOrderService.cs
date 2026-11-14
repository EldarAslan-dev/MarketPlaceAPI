using MarketPlace.Api.DTOs.Orders;

namespace MarketPlace.Api.Services.Interfaces;

public interface IOrderService
{
    Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrderResponse>> GetByBuyerIdAsync(int buyerId, CancellationToken cancellationToken = default);
}
