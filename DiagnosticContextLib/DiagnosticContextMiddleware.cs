using Microsoft.AspNetCore.Http;

namespace DiagnosticContextLib;

public class DiagnosticContextMiddleware
{
    private readonly RequestDelegate _next;

    public DiagnosticContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.ToString();
        if (path == "/metrics")
        {
            await _next(context);
            return;
        }
        DiagnosticContext.SetEntryPoint(context.Request.Path.ToString());
        await _next(context);
    }
}