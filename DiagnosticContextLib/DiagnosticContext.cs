using System.Diagnostics;
using Prometheus;

namespace DiagnosticContextLib
{
    public class DiagnosticContext : IDiagnosticContext
    {
        private static readonly AsyncLocal<string> EntryPoint = new();

        private readonly Gauge _metricGaugeDuration = Metrics.CreateGauge(
            "method_execution_duration",
            "Tracks the duration of method executions in milliseconds",
            new GaugeConfiguration
            {
                LabelNames = new[] { "method_name", "entry_point" }
            }
        );

        public static void SetEntryPoint(string entryPoint)
        {
            EntryPoint.Value = entryPoint;
        }

        public IDisposable Measure(string methodName)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            return new DisposableAction(() =>
            {
                stopwatch.Stop();
                var duration = stopwatch.ElapsedMilliseconds;
                _metricGaugeDuration.WithLabels(methodName, EntryPoint.Value).Set(duration);
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