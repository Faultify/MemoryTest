Memory Test presents an easy way to test dotnet assemblies, without external processes, in memory. This is achieved by invoking XUnit or NUnit directly instead of using `VsConsole` or `DotnetTest`. Memory Test provides a simple to use abstraction while at the same time exposing underlying framework details for usability purposes. 

## Features

- [X] XUnit Support
- [X] NUnit Support

_the support for test assembly versions depends on backwards compadibility of the test host, memory test uses the newest versions_

## Getting Started

- For an example check the [console demo](https://github.com/Faultify/MemoryTest/blob/main/Faultify.MemoryTest.Console/Program.cs). 

**1. Instantiate a Test Host Runner**

```csharp
 var testHostRunner = new XUnitTestHostRunner("path to test assembly"); 
 var testHostRunner = new NUnitTestHostRunner("path to test assembly");  
```

**2. Register Event Handlers**

```csharp
testHostRunner.TestStart += OnTestStart;
testHostRunner.TestEnd += OnTestEnd;
testHostRunner.TestSessionStart += OnTestSessionStart;
testHostRunner.TestSessionEnd += OnTestSessionEnd;

private static void OnTestSessionStart(object? sender, TestSessionStart e)
{
    Console.WriteLine($"Test session start [{e.StartTime:hh:mm:ss t z}]");
}

private static void OnTestSessionEnd(object? sender, TestSessionEnd e)
{
    Console.WriteLine(
        $"[{e.EndTime:hh:mm:ss t z}]: Passed: {e.Passed}, Failed: {e.FailedTests}, Skipped: {e.Skipped}, Run Result: {e.TestOutcome}");
}

private static void OnTestEnd(object? sender, TestEnd e)
{
    Console.WriteLine($"[{e.TestOutcome}] Test Finish [{e.StartTime:hh:mm:ss t z}/{e.EndTime:hh:mm:ss t z}]: {e.TypeName} | {e.TestName}");
}

private static void OnTestStart(object? sender, TestStart e)
{
    Console.WriteLine($"Test case start: {e.TypeName} | {e.TestName}");
}
```

**3. Run The Test Host**

```csharp
 testHostRunner.RunTestsAsync(CancellationToken.None).Wait(); // or use await
```

**4. Done**
That's it. Enjoy testing!

## Configuring
NUnit: 
1. Provide [Settings](https://github.com/Faultify/MemoryTest/blob/main/Faultify.MemoryTest.NUnit/NUnitTestHostRunner.cs#L23)
2. Add [NUnit Config File](https://docs.nunit.org/articles/nunit/technical-notes/usage/Configuration-Files.html) to the test assembly directory.

XUnit:

1.Add [XUnit Config File](https://xunit.net/docs/configuration-files) to the test assembly directory.

