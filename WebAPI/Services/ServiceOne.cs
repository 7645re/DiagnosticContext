using DiagnosticContextLib;
using WebAPI.Repositories;

namespace WebAPI.Services;

public class ServiceOne : IServiceOne
{
    private readonly IRepositoryOne _repositoryOne;
    private readonly IRepositoryTwo _repositoryTwo;
    private readonly IDiagnosticContext _diagnosticContext;

    public ServiceOne(
        IDiagnosticContext diagnosticContext,
        IRepositoryOne repositoryOne,
        IRepositoryTwo repositoryTwo)
    {
        _diagnosticContext = diagnosticContext;
        _repositoryOne = repositoryOne;
        _repositoryTwo = repositoryTwo;
    }

    public async Task MethodOne()
    {
        using (_diagnosticContext.Measure($"{nameof(ServiceOne)}.{nameof(MethodOne)}"))
        {
            await Task.Delay(new Random().Next(1000, 3000));
            await _repositoryOne.MethodOne();
        }
    }

    public async Task MethodTwo()
    {
        using (_diagnosticContext.Measure($"{nameof(ServiceOne)}.{nameof(MethodTwo)}"))
        {
            await Task.Delay(new Random().Next(1000, 3000));
            await _repositoryTwo.MethodTwo();
            await _repositoryOne.MethodTwo();
        }
    }
    
    public async Task MethodThree()
    {
        using (_diagnosticContext.Measure($"{nameof(ServiceOne)}.{nameof(MethodThree)}"))
        {
            await Task.Delay(new Random().Next(1000, 3000));
            await _repositoryOne.MethodOne();
        }
    }
}