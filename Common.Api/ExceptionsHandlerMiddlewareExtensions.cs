using Microsoft.AspNetCore.Builder;

namespace Common.Service
{
    public static class ExceptionsHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionsHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionsHandlerMiddleware>();

            return app;
        }
    }
}
