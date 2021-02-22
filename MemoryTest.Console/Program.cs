using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Faultify.MemoryTest.NUnit;
using Faultify.MemoryTest.TestInformation;
using Faultify.MemoryTest.XUnit;

namespace Faultify.MemoryTest.Console
{
    internal class Program
    {
        private static string _impersonationResolverPath = "";

        private static readonly StringBuilder Sb = new StringBuilder();

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Ignore missing resources
            if (args.Name.Contains(".resources"))
                return null;

            // check for assemblies already loaded
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
                return assembly;

            // Try to load by filename - split out the filename of the full assembly name
            // and append the base path of the original assembly (ie. look in the same dir)
            var filename = args.Name.Split(',')[0] + ".dll".ToLower();

            var asmFile = Path.Combine(_impersonationResolverPath, filename);

            try
            {
                return Assembly.LoadFrom(asmFile);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            _impersonationResolverPath =
                @"E:\programming\FaultifyNew\Faultify\Benchmark\Faultify.Benchmark.XUnit\bin\Debug\netcoreapp3.1\";

            var xunitHostRunner =
                new XUnitTestHostRunner(
                    @"E:\programming\FaultifyNew\Faultify\Benchmark\Faultify.Benchmark.XUnit\bin\Debug\netcoreapp3.1\Faultify.Benchmark.XUnit.dll");
            xunitHostRunner.TestStart += OnTestStart;
            xunitHostRunner.TestEnd += OnTestEnd;
            xunitHostRunner.TestSessionStart += OnTestSessionStart;
            xunitHostRunner.TestSessionEnd += OnTestSessionEnd;

            xunitHostRunner.RunTestsAsync(CancellationToken.None).Wait();

            System.Console.WriteLine(xunitHostRunner.Output.ToString());
            System.Console.WriteLine(xunitHostRunner.Error.ToString());

            _impersonationResolverPath =
                @"E:\programming\FaultifyNew\Faultify\Benchmark\Faultify.Benchmark.NUnit\bin\Debug\netcoreapp3.1\";

            var nunitHostRunner =
                new NUnitTestHostRunner(
                    @"E:\programming\FaultifyNew\Faultify\Benchmark\Faultify.Benchmark.NUnit\bin\Debug\netcoreapp3.1\Faultify.Benchmark.NUnit.dll");
            nunitHostRunner.TestStart += OnTestStart;
            nunitHostRunner.TestEnd += OnTestEnd;
            nunitHostRunner.TestSessionStart += OnTestSessionStart;
            nunitHostRunner.TestSessionEnd += OnTestSessionEnd;

            nunitHostRunner.RunTestsAsync(CancellationToken.None).Wait();

            System.Console.WriteLine(Sb.ToString());
        }

        private static void OnTestSessionStart(object? sender, TestSessionStart e)
        {
            Sb.AppendLine($"\n\n==== Test session start [{e.StartTime:hh:mm:ss t z}] ====");
        }

        private static void OnTestSessionEnd(object? sender, TestSessionEnd e)
        {
            Sb.AppendLine(
                $"==== Test session end [{e.EndTime:hh:mm:ss t z}]: Passed: {e.Passed}, Failed: {e.FailedTests}, Skipped: {e.Skipped}, Run Result: {e.TestOutcome} ====\t");
        }

        private static void OnTestEnd(object? sender, TestEnd e)
        {
            Sb.AppendLine(
                $"[{e.TestOutcome}] Test Finish [{e.StartTime:hh:mm:ss t z}/{e.EndTime:hh:mm:ss t z}]: {e.TypeName} | {e.TestName}");
        }

        private static void OnTestStart(object? sender, TestStart e)
        {
            Sb.AppendLine($"Test case start: {e.TypeName} | {e.TestName}");
        }
    }
}