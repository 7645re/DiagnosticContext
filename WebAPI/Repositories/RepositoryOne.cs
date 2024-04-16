using DiagnosticContextLib;

namespace WebAPI.Repositories;

public class RepositoryOne : IRepositoryOne
{
    private readonly IDiagnosticContext _diagnosticContext;

    public RepositoryOne(IDiagnosticContext diagnosticContext)
    {
        _diagnosticContext = diagnosticContext;
    }

    public async Task MethodOne()
    {
        using (_diagnosticContext.Measure($"{nameof(RepositoryOne)}.{nameof(MethodOne)}"))
        {
            await Task.Delay(new Random().Next(1000, 3000));
        }
    }

    public async Task MethodTwo()
    {
        using (_diagnosticContext.Measure($"{nameof(RepositoryOne)}.{nameof(MethodTwo)}"))
        {
            await Task.Delay(new Random().Next(1000, 3000));
        }
    }
}