using System.Diagnostics;
using System.Runtime.CompilerServices;
using Prometheus;

namespace DiagnosticContext;

public class DiagnosticContext
{
    private readonly Counter _metricCounterDuration = Metrics.CreateCounter(
        "method_execution_duration",
        "Counts the duration of method executions in milliseconds",
        new CounterConfiguration
        {
            LabelNames = new[]
            {
                "method_name",
                "entry_point"
            },
        }
    );

    private readonly Counter _metricCounterExecution = Metrics.CreateCounter(
        "method_execution_count",
        "Counts the number of method executions",
        new CounterConfiguration
        {
            LabelNames = new[]
            {
                "method_name",
                "entry_point"
            }
        }
    );

    private readonly string _entryPoint;

    public DiagnosticContext(string entryPoint)
    {
        _entryPoint = entryPoint;
    }

    public IDisposable Measure([CallerMemberName] string methodName = "")
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        return new DisposableAction(() =>
        {
            stopwatch.Stop();
            _metricCounterDuration.WithLabels(methodName, _entryPoint).Inc(stopwatch.ElapsedMilliseconds);
            _metricCounterExecution.WithLabels(methodName, _entryPoint).Inc();
        });
    }

    private class DisposableAction : IDisposable
    {
        private readonly Action _action;

        public DisposableAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action.Invoke();
        }
    }
}