using MarketPlace.Api.Data;
using MarketPlace.Api.Entities;
using MarketPlace.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Api.Repositories;

public sealed class OrderRepository(AppDbContext db) : IOrderRepository
{
    public async Task AddAsync(Order order, CancellationToken cancellationToken = default) =>
        await db.Orders.AddAsync(order, cancellationToken);

    public async Task<IReadOnlyList<Order>> GetByBuyerIdAsync(int buyerId, CancellationToken cancellationToken = default) =>
        await db.Orders.AsNoTracking().Include(x => x.Product)
            .Where(x => x.BuyerId == buyerId).OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id).ToListAsync(cancellationToken);
}
