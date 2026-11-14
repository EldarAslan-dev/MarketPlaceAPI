using MarketPlace.Api.Entities;

namespace MarketPlace.Api.Repositories.Interfaces;

public interface IStoreRepository
{
    Task<Store?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Store>> ListAsync(CancellationToken cancellationToken = default);
    Task<bool> NameExistsAsync(int ownerId, string name, CancellationToken cancellationToken = default);
    Task AddAsync(Store store, CancellationToken cancellationToken = default);
}
