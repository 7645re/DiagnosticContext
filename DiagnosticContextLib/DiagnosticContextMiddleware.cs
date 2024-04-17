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
        DiagnosticContext.SetEntryPoint(context.Request.Path.ToString());
        await _next(context);
    }
}