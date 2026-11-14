using MarketPlace.Api.DTOs.Categories;

namespace MarketPlace.Api.Services.Interfaces;

public interface ICategoryService
{
    Task<CategoryResponse> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CategoryResponse>> ListAsync(CancellationToken cancellationToken = default);
}
