using MarketPlace.Api.Data;
using MarketPlace.Api.DTOs.Users;
using MarketPlace.Api.Entities;
using MarketPlace.Api.Exceptions;
using MarketPlace.Api.Repositories.Interfaces;
using MarketPlace.Api.Services.Interfaces;

namespace MarketPlace.Api.Services;

public sealed class UserService(IUserRepository users, IUnitOfWork unitOfWork) : IUserService
{
    public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(request.Role))
            throw new ApiException(400, "invalid_role", "User role is invalid.");

        var email = request.Email.Trim().ToLowerInvariant();
        var fullName = request.FullName.Trim();

        if (fullName.Length < 2)
            throw new ApiException(400, "invalid_name", "Full name must contain at least 2 characters.");

        if (await users.EmailExistsAsync(email, cancellationToken))
            throw new ApiException(409, "email_exists", "Email is already in use.");

        var user = new User { FullName = fullName, Email = email, Role = request.Role };
        await users.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new UserResponse(user.Id, user.FullName, user.Email, user.Role);
    }

    public async Task<UserResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await users.GetByIdAsync(id, cancellationToken)
            ?? throw new ApiException(404, "user_not_found", "User was not found.");

        return new UserResponse(user.Id, user.FullName, user.Email, user.Role);
    }
}
