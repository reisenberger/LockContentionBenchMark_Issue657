using System.Linq;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;

namespace ConcurrentDictionaryLockContention
{
    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            var baseJob = Job.ShortRun
                .WithIterationCount(50)
                .WithLaunchCount(1)
                /*.WithWarmupCount(1)*/
                .WithId($"Contention{Benchmarks.ParallelTasks}");

            Add(baseJob);

            /*Add(MemoryDiagnoser.Default);*/
            Add(MarkdownExporter.GitHub);

            Add(DefaultConfig.Instance.GetColumnProviders().ToArray());
            Add(DefaultConfig.Instance.GetLoggers().ToArray());
            Add(CsvExporter.Default);
            Add(MarkdownExporter.GitHub);
        }
    }
}
