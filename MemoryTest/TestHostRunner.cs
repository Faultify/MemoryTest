using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Faultify.MemoryTest.TestInformation;

namespace Faultify.MemoryTest
{
    /// <summary>
    /// Base class for a test host runner. 
    /// </summary>
    public abstract class TestHostRunner : ITestHostRunner
    {
        protected TestHostRunner(string testProjectAssemblyPath)
        {
            TestProjectAssemblyPath = testProjectAssemblyPath;
            Error = new StringBuilder();
            Output = new StringBuilder();
        }

        /// <summary>
        /// The error log from a test run. 
        /// </summary>
        public StringBuilder Error { get; }

        /// <summary>
        /// The output log from a test run.
        /// </summary>

        public StringBuilder Output { get; }

        /// <summary>
        /// The absolute full path to the assembly under test. 
        /// </summary>
        public string TestProjectAssemblyPath { get; }

        /// <summary>
        /// Event that fires when a test starts.
        /// </summary>
        public event EventHandler<TestStart> TestStart;

        /// <summary>
        /// Event that fires when a test ends.
        /// </summary>
        public event EventHandler<TestEnd> TestEnd;

        /// <summary>
        /// Event that fires when a test session starts.
        /// </summary>
        public event EventHandler<TestSessionStart> TestSessionStart;

        /// <summary>
        /// Event that fires when a test session ends.
        /// </summary>
        public event EventHandler<TestSessionEnd> TestSessionEnd;
        
        public abstract Task RunTestsAsync(CancellationToken token, HashSet<string> tests = null);

        /// <summary>
        /// Invokes the test start event.
        /// </summary>
        /// <param name="testStart"></param>
        public void OnTestCaseStart(TestStart testStart)
        {
            TestStart?.Invoke(null, testStart);
        }

        /// <summary>
        /// Invokes the test end event.
        /// </summary>
        /// <param name="testEnd"></param>
        public void OnTestCaseEnd(TestEnd testEnd)
        {
            TestEnd?.Invoke(this, testEnd);
        }

        /// <summary>
        /// Invokes the test session start event.
        /// </summary>
        public void OnTestSessionStart(TestSessionStart testSessionStart)
        {
            TestSessionStart?.Invoke(this, testSessionStart);
        }

        /// <summary>
        /// Invokes the test session end event.
        /// </summary>
        public void OnTestSessionEnd(TestSessionEnd testSessionEnd)
        {
            TestSessionEnd?.Invoke(this, testSessionEnd);
        }

        /// <summary>
        /// Logs the given output. 
        /// </summary>
        public void LogOutput(string output)
        {
            Output.AppendLine(output);
        }

        /// <summary>
        /// Logs the given error.
        /// </summary>
        /// <param name="error"></param>
        public void LogError(string error)
        {
            Error.AppendLine(error);
        }
    }
}