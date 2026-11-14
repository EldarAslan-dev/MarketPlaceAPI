using MarketPlace.Api.Data;
using MarketPlace.Api.DTOs.Stores;
using MarketPlace.Api.Entities;
using MarketPlace.Api.Exceptions;
using MarketPlace.Api.Repositories.Interfaces;
using MarketPlace.Api.Services.Interfaces;

namespace MarketPlace.Api.Services;

public sealed class StoreService(IStoreRepository stores, IUserRepository users, IUnitOfWork unitOfWork) : IStoreService
{
    public async Task<StoreResponse> CreateAsync(CreateStoreRequest request, CancellationToken cancellationToken = default)
    {
        var owner = await users.GetByIdAsync(request.OwnerId, cancellationToken)
            ?? throw new ApiException(404, "user_not_found", "Owner was not found.");

        if (owner.Role != UserRole.Seller)
            throw new ApiException(403, "seller_required", "Only sellers can create stores.");

        var name = request.Name.Trim();
        if (name.Length < 2)
            throw new ApiException(400, "invalid_name", "Store name must contain at least 2 characters.");

        if (await stores.NameExistsAsync(owner.Id, name, cancellationToken))
            throw new ApiException(409, "store_exists", "Seller already has a store with this name.");

        var store = new Store { OwnerId = owner.Id, Name = name };
        await stores.AddAsync(store, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new StoreResponse(store.Id, store.Name, owner.Id, owner.FullName, store.CreatedAt);
    }

    public async Task<StoreResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var store = await stores.GetByIdAsync(id, cancellationToken)
            ?? throw new ApiException(404, "store_not_found", "Store was not found.");
        return Map(store);
    }

    public async Task<IReadOnlyList<StoreResponse>> ListAsync(CancellationToken cancellationToken = default) =>
        (await stores.ListAsync(cancellationToken)).Select(Map).ToList();

    private static StoreResponse Map(Store store) =>
        new(store.Id, store.Name, store.OwnerId, store.Owner.FullName, store.CreatedAt);
}
