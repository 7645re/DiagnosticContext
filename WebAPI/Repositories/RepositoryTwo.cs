using DiagnosticContextLib;

namespace WebAPI.Repositories;

public class RepositoryTwo : IRepositoryTwo
{
    private readonly IDiagnosticContext _diagnosticContext;

    public RepositoryTwo(IDiagnosticContext diagnosticContext)
    {
        _diagnosticContext = diagnosticContext;
    }

    public async Task MethodOne()
    {
        using (_diagnosticContext.Measure($"{nameof(RepositoryTwo)}.{nameof(MethodOne)}"))
        {
            await Task.Delay(new Random().Next(1000, 3000));
        }
    }

    public async Task MethodTwo()
    {
        using (_diagnosticContext.Measure($"{nameof(RepositoryTwo)}.{nameof(MethodTwo)}"))
        {
            await Task.Delay(new Random().Next(1000, 3000));
        }
    }
}