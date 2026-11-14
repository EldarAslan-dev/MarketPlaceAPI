using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Api.DTOs.Categories;

public sealed class CreateCategoryRequest
{
    [Required, StringLength(100, MinimumLength = 2)]
    public required string Name { get; init; }
}
