using DiagnosticContextLib;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ControllerTwo : ControllerBase
{
    private readonly IDiagnosticContext _diagnosticContext;
    private readonly IServiceOne _serviceOne;
    private readonly IServiceTwo _serviceTwo;
    public ControllerTwo(
        IDiagnosticContext diagnosticContext,
        IServiceTwo serviceTwo,
        IServiceOne serviceOne)
    {
        _diagnosticContext = diagnosticContext;
        _serviceTwo = serviceTwo;
        _serviceOne = serviceOne;
    }

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
    
    [HttpGet("MethodTwo")]
    public async Task<IActionResult> MethodTwo()
    {
        using (_diagnosticContext.Measure($"{nameof(ControllerOne)}.{nameof(MethodTwo)}"))
        {
            await _serviceOne.MethodTwo();
            await _serviceTwo.MethodTwo();
        }
        return Ok();
    }
}