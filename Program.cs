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
    private class Wrapper
    {
        public Wrapper(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }

    private List<Wrapper> my { get; set; } = new() { new Wrapper("1"), new Wrapper("2"), new Wrapper("3"), new Wrapper("4"), new Wrapper("5"), new Wrapper("6"), new Wrapper("7"), new Wrapper("8") };
    private List<Wrapper> employee { get; set; } = new() { new Wrapper("3"), new Wrapper("4") };

    [Benchmark]
    public bool BigToSmallContains()
    {
        return my.Any(c => employee.Select(x => x.Id).Contains(c.Id));
    }

    [Benchmark]
    public bool BigToSmallWhere()
    {
        return my.Where(c => employee.Select(x => x.Id).Contains(c.Id)).Any();
    }

    [Benchmark]
    public bool BigToSmallAny()
    {
        return my.Any(c => employee.Any(x => x.Id == c.Id));
    }

    [Benchmark]
    public bool SmallToBigContains()
    {
        return employee.Any(c => my.Select(x => x.Id).Contains(c.Id));
    }

    [Benchmark]
    public bool SmallToBigWhere()
    {
        return employee.Where(c => my.Select(x => x.Id).Contains(c.Id)).Any();
    }

    [Benchmark]
    public bool SmallToBigAny()
    {
        return employee.Any(c => my.Any(x => x.Id == c.Id));
    }

    [Benchmark]
    public bool Dictionary()
    {
        var dict = employee.ToDictionary(x => x.Id, x => x);

        return my.Any(c => dict.ContainsKey(c.Id));
    }
}
