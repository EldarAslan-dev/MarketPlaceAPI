using MarketPlace.Api.Data;
using MarketPlace.Api.DTOs.Categories;
using MarketPlace.Api.Entities;
using MarketPlace.Api.Exceptions;
using MarketPlace.Api.Repositories.Interfaces;
using MarketPlace.Api.Services.Interfaces;

namespace MarketPlace.Api.Services;

public sealed class CategoryService(ICategoryRepository categories, IUnitOfWork unitOfWork) : ICategoryService
{
    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var name = request.Name.Trim();
        if (name.Length < 2)
            throw new ApiException(400, "invalid_name", "Category name must contain at least 2 characters.");

        if (await categories.NameExistsAsync(name, cancellationToken))
            throw new ApiException(409, "category_exists", "Category already exists.");

        var category = new Category { Name = name };
        await categories.AddAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new CategoryResponse(category.Id, category.Name);
    }

    public async Task<IReadOnlyList<CategoryResponse>> ListAsync(CancellationToken cancellationToken = default) =>
        (await categories.ListAsync(cancellationToken))
            .Select(category => new CategoryResponse(category.Id, category.Name)).ToList();
}
