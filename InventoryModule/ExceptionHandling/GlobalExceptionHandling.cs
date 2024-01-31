using InventoryModule.Dtos;
using Microsoft.AspNetCore.Diagnostics;

namespace InventoryModule.ExceptionHandling
{
    public class GlobalExceptionHandling : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ResponseModel<object> responseModel = new ResponseModel<object>
            {
                Error = "Exception occured.",
                Message = exception.Message
            };

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(responseModel);

            return true;
        }
    }
}
