using DiagnosticContextLib;
using WebAPI.Repositories;

namespace WebAPI.Services;

public class ServiceTwo : IServiceTwo
{
    private readonly IDiagnosticContext _diagnosticContext;
    private readonly IRepositoryOne _repositoryOne;
    private readonly IRepositoryTwo _repositoryTwo;

    public ServiceTwo(
        IDiagnosticContext diagnosticContext,
        IRepositoryTwo repositoryTwo,
        IRepositoryOne repositoryOne)
    {
        _diagnosticContext = diagnosticContext;
        _repositoryTwo = repositoryTwo;
        _repositoryOne = repositoryOne;
    }

    public async Task MethodOne()
    {
        using (_diagnosticContext.Measure($"{nameof(ServiceTwo)}.{nameof(MethodOne)}"))
        {
            await Task.Delay(new Random().Next(1000, 3000));
            await _repositoryTwo.MethodOne();
            await _repositoryOne.MethodOne();
        }
    }

    public async Task MethodTwo()
    {
        using (_diagnosticContext.Measure($"{nameof(ServiceTwo)}.{nameof(MethodTwo)}"))
        {
            await Task.Delay(new Random().Next(1000, 3000));
            await _repositoryTwo.MethodTwo();
        }
    }
}