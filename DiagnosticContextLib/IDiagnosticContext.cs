namespace DiagnosticContextLib;

public interface IDiagnosticContext
{
    IDisposable Measure(string methodName = "");
}