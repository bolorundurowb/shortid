using BenchmarkDotNet.Attributes;

namespace shortid.Benchmarks;

/// <summary>
/// Throughput vs length (default-style pool: letters + specials, no numbers).
/// </summary>
[MemoryDiagnoser]
public class ShortIdLengthBenchmarks
{
    [Params(8, 9, 10, 11, 12, 13, 14, 15)]
    public int Length { get; set; }

    private ShortIdOptions _options = null!;

    [GlobalSetup]
    public void Setup()
    {
        ShortId.Reset();
        _options = new ShortIdOptions(useNumbers: false, useSpecialCharacters: true, length: Length);
    }

    [Benchmark]
    public string Generate() => ShortId.Generate(_options);
}
