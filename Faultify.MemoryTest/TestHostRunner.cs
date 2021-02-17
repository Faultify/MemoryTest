using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Faultify.MemoryTest.TestInformation;

namespace Faultify.MemoryTest
{
    public abstract class TestHostRunner : ITestHostRunner
    {
        public StringBuilder Error;

        public StringBuilder Output;

        protected TestHostRunner(string testProjectAssemblyPath)
        {
            TestProjectAssemblyPath = testProjectAssemblyPath;
            Error = new StringBuilder();
            Output = new StringBuilder();
        }

        public string TestProjectAssemblyPath { get; }

        public abstract Task RunTestsAsync(CancellationToken token, HashSet<string> tests = null);

        public event EventHandler<TestStart> TestCaseStart;
        public event EventHandler<TestEnd> TestCaseEnd;
        public event EventHandler<TestSessionStart> TestSessionStart;
        public event EventHandler<TestSessionEnd> TestSessionEnd;

        public void OnTestCaseStart(TestStart testStart)
        {
            TestCaseStart?.Invoke(null, testStart);
        }

        public void OnTestCaseEnd(TestEnd testEnd)
        {
            TestCaseEnd?.Invoke(this, testEnd);
        }

        public void OnTestSessionStart(TestSessionStart testSessionStart)
        {
            TestSessionStart?.Invoke(this, testSessionStart);
        }

        public void OnTestSessionEnd(TestSessionEnd testSessionEnd)
        {
            TestSessionEnd?.Invoke(this, testSessionEnd);
        }

        public void LogOutput(string output)
        {
            Output.AppendLine(output);
        }

        public void LogError(string error)
        {
            Error.AppendLine(error);
        }
    }
}