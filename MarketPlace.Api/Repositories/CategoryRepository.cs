using MarketPlace.Api.Data;
using MarketPlace.Api.Entities;
using MarketPlace.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Api.Repositories;

public sealed class CategoryRepository(AppDbContext db) : ICategoryRepository
{
    public Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        db.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Category>> ListAsync(CancellationToken cancellationToken = default) =>
        await db.Categories.AsNoTracking().OrderBy(x => x.Name).ToListAsync(cancellationToken);

    public Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default) =>
        db.Categories.AnyAsync(x => x.Name == name, cancellationToken);

    public async Task AddAsync(Category category, CancellationToken cancellationToken = default) =>
        await db.Categories.AddAsync(category, cancellationToken);
}
