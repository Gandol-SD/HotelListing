using HotelListing.API.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace HotelListing.API.Middlewares
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionsMiddleware> logger;

        public ExceptionsMiddleware(RequestDelegate next, ILogger<ExceptionsMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            try
            {
                await next(ctx);
            }
            catch (Exception ex)
            {
                await handExceptionAsync(ctx, ex);
            }
        }

        public Task handExceptionAsync(HttpContext ctx, Exception ex)
        {
            ctx.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            ErrorDetails errorDetails = new ErrorDetails
            {
                type = "Failure",
                message = ex.Message,
            };

            switch (ex)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errorDetails.type = "Not Found";
                    break;
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    errorDetails.type = "UnAuthorized";
                    break;
                default: break;
            }
            string response = JsonConvert.SerializeObject(errorDetails);
            ctx.Response.StatusCode = (int)statusCode;

            logger.LogError($"ExceptionMiddleWare : StatusCode-{statusCode} Msg:{errorDetails.message} ");

            return ctx.Response.WriteAsync(response);
        }
    }
}
