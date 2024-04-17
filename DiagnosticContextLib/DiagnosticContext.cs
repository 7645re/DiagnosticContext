using System.Diagnostics;
using Prometheus;

namespace DiagnosticContextLib
{
    public class DiagnosticContext : IDiagnosticContext
    {
        private string? _entryPoint;

        private readonly Gauge _metricGaugeDuration = Metrics.CreateGauge(
            "method_execution_duration",
            "Tracks the duration of method executions in milliseconds",
            new GaugeConfiguration
            {
                LabelNames = new[] { "method_name", "entry_point" }
            }
        );

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
}