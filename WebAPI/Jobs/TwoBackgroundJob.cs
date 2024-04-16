using DiagnosticContextLib;
using WebAPI.Services;

namespace WebAPI.Jobs;

public class TwoBackgroundJob : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TwoBackgroundJob(
        IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellation)
    {
        while (!cancellation.IsCancellationRequested)
        {
            await Task.Delay(new Random().Next(1000, 5000), cancellation);

            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var serviceOne = scope.ServiceProvider.GetRequiredService<IServiceOne>();
            var serviceTwo = scope.ServiceProvider.GetRequiredService<IServiceTwo>();
            var diagnosticContext = scope.ServiceProvider.GetRequiredService<IDiagnosticContext>();
            
            DiagnosticContext.SetEntryPoint($"{nameof(TwoBackgroundJob)}.{nameof(ExecuteAsync)}");
            using (diagnosticContext.Measure($"{nameof(TwoBackgroundJob)}.{nameof(ExecuteAsync)}"))
            {
                await serviceOne.MethodTwo();
                await serviceTwo.MethodTwo();
                await Task.Delay(new Random().Next(100, 500), cancellation);
            }
        }
    }
}