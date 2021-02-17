using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using NUnit.Engine;

namespace Faultify.MemoryTest.NUnit
{
    public class NUnitTestHostRunner : TestHostRunner
    {
        public NUnitTestHostRunner(string testProjectAssemblyPath) : base(testProjectAssemblyPath)
        {
        }

        public override async Task RunTestsAsync(CancellationToken token, HashSet<string> tests = null)
        {
            // Get an interface to the engine
            using var engine = TestEngineActivator.CreateInstance();
            engine.WorkDirectory = new FileInfo(TestProjectAssemblyPath).DirectoryName;

            // Create a simple test package - one assembly, no special settings
            var package = new TestPackage(TestProjectAssemblyPath);
            package.AddSetting("BaseDirectory", new FileInfo(TestProjectAssemblyPath).DirectoryName);

            // Get a runner for the test package
            using var runner = engine.GetRunner(package);

            try
            {
                var run = runner.RunAsync(new NUnitEventListener(this), GetTestFilter(tests));

                await Task.Run(() =>
                {
                    run.Wait(Timeout.Infinite);
                }, token);
            }
            finally
            {
                runner.Dispose();
                engine.Dispose();
            }
        }

        private TestFilter GetTestFilter(IEnumerable<string> tests)
        {
            var testFilterBuilder = new TestFilterBuilder();
            if (tests != null)
            {
                foreach (var test in tests) testFilterBuilder.AddTest(test);
            }

            return testFilterBuilder.GetFilter();
        }
    }
}