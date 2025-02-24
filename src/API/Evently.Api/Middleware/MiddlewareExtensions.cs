namespace Evently.Api.Middleware;

internal static class MiddlewareExtensions
{
    internal static IApplicationBuilder UseLogContext(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LogContextTraceLoggingMiddleware>();
    }
}
