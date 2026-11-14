using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Api.DTOs.Stores;

public sealed class CreateStoreRequest
{
    [Range(1, int.MaxValue)]
    public int OwnerId { get; init; }

    [Required, StringLength(150, MinimumLength = 2)]
    public required string Name { get; init; }
}
