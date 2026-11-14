namespace MarketPlace.Api.DTOs.Stores;

public sealed record StoreResponse(int Id, string Name, int OwnerId, string OwnerName, DateTimeOffset CreatedAt);
