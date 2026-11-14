using MarketPlace.Api.DTOs.Stores;

namespace MarketPlace.Api.Services.Interfaces;

public interface IStoreService
{
    Task<StoreResponse> CreateAsync(CreateStoreRequest request, CancellationToken cancellationToken = default);
    Task<StoreResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StoreResponse>> ListAsync(CancellationToken cancellationToken = default);
}
