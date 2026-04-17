using BenchmarkDotNet.Attributes;

namespace shortid.Benchmarks;

/// <summary>
/// Effect of each option at fixed length 10.
/// </summary>
public class ShortIdOptionsBenchmarks
{
    private const int FixedLength = 10;

    private ShortIdOptions _lettersOnly = null!;
    private ShortIdOptions _lettersAndSpecials = null!;
    private ShortIdOptions _lettersAndNumbers = null!;
    private ShortIdOptions _fullCharset = null!;
    private ShortIdOptions _sequentialDefault = null!;

    [GlobalSetup]
    public void Setup()
    {
        ShortId.Reset();
        _lettersOnly = new ShortIdOptions(false, false, FixedLength);
        _lettersAndSpecials = new ShortIdOptions(false, true, FixedLength);
        _lettersAndNumbers = new ShortIdOptions(true, false, FixedLength);
        _fullCharset = new ShortIdOptions(true, true, FixedLength);
        _sequentialDefault = new ShortIdOptions(false, true, FixedLength, generateSequential: true);
    }

    [Benchmark(Baseline = true)]
    public string LettersOnly() => ShortId.Generate(_lettersOnly);

    [Benchmark]
    public string LettersAndSpecials() => ShortId.Generate(_lettersAndSpecials);

    [Benchmark]
    public string LettersAndNumbers() => ShortId.Generate(_lettersAndNumbers);

    [Benchmark]
    public string FullCharset() => ShortId.Generate(_fullCharset);

    [Benchmark]
    public string Sequential() => ShortId.Generate(_sequentialDefault);
}
