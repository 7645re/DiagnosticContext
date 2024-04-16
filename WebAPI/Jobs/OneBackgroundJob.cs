using DiagnosticContextLib;
using WebAPI.Services;

namespace WebAPI.Jobs;

public class OneBackgroundJob : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public OneBackgroundJob(
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
            
            DiagnosticContext.SetEntryPoint($"{nameof(OneBackgroundJob)}.{nameof(ExecuteAsync)}");
            using (diagnosticContext.Measure($"{nameof(OneBackgroundJob)}.{nameof(ExecuteAsync)}"))
            {
                await serviceTwo.MethodOne();
                await serviceOne.MethodOne();
                await Task.Delay(new Random().Next(100, 500), cancellation);
            }
        }
    }
}