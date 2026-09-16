using ForgeOps.Api.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ForgeOps.Api.Shared;

public static class ErrorResponse
{
    public static ObjectResult HandleServerError(ILogger _logger, Exception ex, string message)
    {
        _logger.LogError(ex, message);

        return new ObjectResult(new BaseForgeOpsError
        {
            ErrorMessage = message,
            ErrorTimestamp = DateTime.UtcNow
        })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}