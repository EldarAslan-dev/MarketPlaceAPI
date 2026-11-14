using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Api.Exceptions;

public sealed class ApiExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ApiException? apiException = exception switch
        {
            ApiException known => known,
            DbUpdateException { InnerException: SqlException sql } when sql.Number is 2601 or 2627 =>
                new ApiException(StatusCodes.Status409Conflict, "duplicate_resource", "A record with these values already exists."),
            _ => null
        };

        if (apiException is null)
        {
            return false;
        }

        await Results.Problem(
            statusCode: apiException.StatusCode,
            title: ReasonPhrases.GetReasonPhrase(apiException.StatusCode),
            detail: apiException.Message,
            extensions: new Dictionary<string, object?> { ["code"] = apiException.Code }
        ).ExecuteAsync(httpContext);

        return true;
    }
}
