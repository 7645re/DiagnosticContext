# Why was it developed
This code example was designed to increase the observability of how fast certain layers in the pipeline are executed.
<img width="1069" alt="image" src="https://github.com/7645re/DiagnosticContext/assets/89273037/a545b476-1df3-4ed7-9b11-c99046399620">
By seeing how long each layer was executed, you can localize the problem area, which you can try to speed up

# How it works
Due to the fact that Diagnostic Context is injected into dependency injection as scoped, each of the pipelines will have a personal instance throughout its execution
```csharp
builder.Services.AddScoped<IDiagnosticContext, DiagnosticContext>();
```

<img width="869" alt="image" src="https://github.com/7645re/DiagnosticContext/assets/89273037/b23ea37a-ffab-43e7-a960-551b5adae42d">

When the diagnostic context is first called, it will check inside itself whether the entry point has been set so that it can then be filtered by it, the entry point can be said as the name of the entire pipeline.
For example, if we made an http request to our application, the controller will pick it up, and if we marked up the controller layer, then the entry point will be controllerName.ControllerMethodName

```csharp
[HttpGet("MethodOne")]
public async Task<IActionResult> MethodOne()
{
    using (_diagnosticContext.Measure($"{nameof(ControllerOne)}.{nameof(MethodOne)}"))
    {
        await _serviceOne.MethodOne();
        await _serviceTwo.MethodOne();
    }
    return Ok();
}
```

```csharp
public IDisposable Measure(string methodName)
{
    _entryPoint ??= methodName;
    
    var stopwatch = new Stopwatch();
    stopwatch.Start();

    return new DisposableAction(() =>
    {
        stopwatch.Stop();
        var duration = stopwatch.ElapsedMilliseconds;
        _metricGaugeDuration.WithLabels(methodName, _entryPoint).Set(duration);
    });
}
```

# How to launch
Execute the `docker-compose up` command, thus you will launch grafana & prometheus, then separately launch WebAPI first, then launch RequestSpammer.

Request Spammer is needed to create requests to our WebAPI application
