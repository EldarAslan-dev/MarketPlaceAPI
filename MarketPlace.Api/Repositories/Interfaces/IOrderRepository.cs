using MarketPlace.Api.Entities;

namespace MarketPlace.Api.Repositories.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Order>> GetByBuyerIdAsync(int buyerId, CancellationToken cancellationToken = default);
}
