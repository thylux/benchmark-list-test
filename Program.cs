using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Configs;
using Perfolizer.Horology;

[assembly: System.Diagnostics.Debuggable(isJITTrackingEnabled: false, isJITOptimizerDisabled: false)]

BenchmarkRunner.Run<PerfTests>(
    DefaultConfig.Instance
        .AddJob(
            Job.Default
                .WithLaunchCount(1)
                .WithWarmupCount(5)
                .WithIterationCount(20)
                .WithIterationTime(TimeInterval.FromMilliseconds(80))
        ));


public class PerfTests
{
    public List<string> my { get; set; } = new() { "1", "2", "3", "4", "5", "6", "7", "8" };
    public List<string> employee { get; set; } = new() { "3", "4" };

    [Benchmark]
    public bool BigToSmallContains()
    {
        return my.Any(c => employee.Contains(c));
    }

    [Benchmark]
    public bool BigToSmallAny()
    {
        return my.Any(c => employee.Any(x => x == c));
    }

    [Benchmark]
    public bool SmallToBigContains()
    {
        return employee.Any(c => my.Contains(c));
    }

    [Benchmark]
    public bool SmallToBigAny()
    {
        return employee.Any(c => my.Any(x => x == c));
    }

    [Benchmark]
    public bool Dictionary()
    {
        var dict = employee.ToDictionary(x => x, x => x);

        return my.Any(c => dict.ContainsKey(c));
    }
}
