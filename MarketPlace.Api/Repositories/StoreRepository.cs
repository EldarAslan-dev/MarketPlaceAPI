using MarketPlace.Api.Data;
using MarketPlace.Api.Entities;
using MarketPlace.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Api.Repositories;

public sealed class StoreRepository(AppDbContext db) : IStoreRepository
{
    public Task<Store?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        db.Stores.AsNoTracking().Include(x => x.Owner)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Store>> ListAsync(CancellationToken cancellationToken = default) =>
        await db.Stores.AsNoTracking().Include(x => x.Owner)
            .OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<bool> NameExistsAsync(int ownerId, string name, CancellationToken cancellationToken = default) =>
        db.Stores.AnyAsync(x => x.OwnerId == ownerId && x.Name == name, cancellationToken);

    public async Task AddAsync(Store store, CancellationToken cancellationToken = default) =>
        await db.Stores.AddAsync(store, cancellationToken);
}
