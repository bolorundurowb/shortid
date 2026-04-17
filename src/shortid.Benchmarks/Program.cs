using BenchmarkDotNet.Running;

// Non-interactive: BenchmarkSwitcher prompts unless benchmarks are selected.
var runArgs = args.Any(static a => a is "--filter" or "-f")
    ? args.ToList()
    : new List<string> { "--filter", "*" }.Concat(args).ToList();
BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(runArgs.ToArray());
