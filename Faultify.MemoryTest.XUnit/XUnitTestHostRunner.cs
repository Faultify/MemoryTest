using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Faultify.MemoryTest.TestInformation;
using Xunit.Runners;

namespace Faultify.MemoryTest.XUnit
{
    /// <summary>
    /// XUnit in memory assembly test runner. 
    /// </summary>
    public class XUnitTestHostRunner : TestHostRunner
    {
        private static readonly ManualResetEvent TestSessionFinished = new ManualResetEvent(false);

        private readonly Dictionary<string, DateTime> _testDurations = new Dictionary<string, DateTime>();

        public XUnitTestHostRunner(string testProjectAssemblyPath) : base(testProjectAssemblyPath)
        {
        }

        public override async Task RunTestsAsync(CancellationToken token, HashSet<string> tests = null)
        {
            await ExecuteAndUnloadAsync(TestProjectAssemblyPath, tests, token);
        }

        // It is important to mark this method as NoInlining, otherwise the JIT could decide
        // to inline it into the Main method. That could then prevent successful unloading
        // of the plugin because some of the MethodInfo / Type / Plugin.Interface / HostAssemblyLoadContext
        // instances may get lifetime extended beyond the point when the plugin is expected to be
        // unloaded.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private async Task ExecuteAndUnloadAsync(string assemblyPath, HashSet<string> tests, CancellationToken token)
        {
            // Create the unloadable HostAssemblyLoadContext
            var alc = new CustomAssemblyLoadContext(assemblyPath);

            // Create a weak reference to the AssemblyLoadContext that will allow us to detect
            // when the unload completes.
            var alcWeakRef = new WeakReference(alc);

            try
            {
                // Load the plugin assembly into the HostAssemblyLoadContext.
                // NOTE: the assemblyPath must be an absolute path.
                var assembly = alc.LoadFromAssemblyPath(assemblyPath);

                using var runner = CreateRunner(tests, assembly.Location);
                
                runner.Start();

                TestSessionFinished.WaitOne();
                TestSessionFinished.Reset();

                await WaitRunnerExitAsync(runner, token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // This initiates the unload of the HostAssemblyLoadContext. The actual unloading doesn't happen
            // right away, GC has to kick in later to collect all the stuff.
            alc.Unload();
        }

        private async Task WaitRunnerExitAsync(AssemblyRunner runner, CancellationToken token)
        {
            while (runner.Status != AssemblyRunnerStatus.Idle)
            {
                // Wait for assembly runner to finish.
                // If we try to dispose while runner is executing,
                // it will throw an error.
                await Task.Delay(5, token);

                token.ThrowIfCancellationRequested();
            }
        }

        private AssemblyRunner CreateRunner(HashSet<string> tests, string assemblyPath)
        {
            var runner = AssemblyRunner.WithoutAppDomain(assemblyPath);
            runner.OnExecutionComplete = OnTestSessionEnd;
            runner.OnDiscoveryComplete = OnTestSessionStart;
            
            runner.OnTestFailed = OnTestFailed;
            runner.OnTestPassed = OnTestPassed;
            runner.OnTestSkipped = OnTestSkipped;

            runner.OnTestStarting = OnTestStart;
            runner.OnTestOutput = OnTestOutput;
            runner.OnErrorMessage = OnTestError;

            if (tests != null)
                runner.TestCaseFilter = testCase => tests.Contains(testCase.DisplayName);

            return runner;
        }

        private (DateTime, DateTime) GetTestExecutionTime(string methodName, decimal executionTime)
        {
            var startTime = _testDurations.GetValueOrDefault(methodName);
            return (startTime, startTime + TimeSpan.FromSeconds((double) executionTime));
        }

        private void OnTestSkipped(TestSkippedInfo obj)
        {
            OnTestCaseEnd(new TestEnd(obj.MethodName, obj.TypeName.Split('.').Last(), obj.TestDisplayName,
                obj.TestCollectionDisplayName, TestOutcome.Skipped, DateTime.MinValue, DateTime.MinValue));
        }

        private void OnTestPassed(TestPassedInfo obj)
        {
            var (startTime, endTime) = GetTestExecutionTime(obj.TestDisplayName, obj.ExecutionTime);
            OnTestCaseEnd(new TestEnd(obj.MethodName, obj.TypeName.Split('.').Last(), obj.TestDisplayName,
                obj.TestCollectionDisplayName, TestOutcome.Passed, startTime, endTime));
            LogOutput(obj.Output);
        }

        private void OnTestFailed(TestFailedInfo obj)
        {
            var (startTime, endTime) = GetTestExecutionTime(obj.TestDisplayName, obj.ExecutionTime);
            OnTestCaseEnd(new TestEnd(obj.MethodName, obj.TypeName.Split('.').Last(), obj.TestDisplayName,
                obj.TestCollectionDisplayName, TestOutcome.Failed, startTime, endTime));
            LogError(obj.ExceptionMessage);
        }

        private void OnTestError(ErrorMessageInfo obj)
        {
            LogOutput(obj.ExceptionMessage);
        }

        private void OnTestOutput(TestOutputInfo obj)
        {
            LogOutput(obj.Output);
        }

        private void OnTestStart(TestStartingInfo obj)
        {
            lock (this)
            {
                _testDurations.Add(obj.TestDisplayName, DateTime.Now);
            }

            OnTestCaseStart(new TestStart(obj.TypeName, obj.MethodName, obj.TestDisplayName,
                obj.TestCollectionDisplayName));
        }

        private void OnTestSessionStart(DiscoveryCompleteInfo obj)
        {
            OnTestSessionStart(new TestSessionStart(DateTime.Now));
        }

        private void OnTestSessionEnd(ExecutionCompleteInfo obj)
        {
            TestSessionFinished.Set();
            OnTestSessionEnd(new TestSessionEnd(obj.TotalTests, DateTime.Now,
                obj.TestsFailed == 0 ? TestOutcome.Passed : TestOutcome.Failed, obj.TestsFailed, obj.TestsSkipped,
                obj.TotalTests - obj.TestsFailed - obj.TestsSkipped, ""));
        }
    }
}