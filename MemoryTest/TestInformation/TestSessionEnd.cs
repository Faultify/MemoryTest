using System;

namespace Faultify.MemoryTest.TestInformation
{
    /// <summary>
    /// Information about a test session that finished running.
    /// </summary>
    public readonly struct TestSessionEnd : ITestSessionInfo
    {
        /// <summary>
        /// Number of tests.
        /// </summary>
        public int TestCount { get; }

        /// <summary>
        /// The end time of this test session.k
        /// </summary>
        public DateTime EndTime { get; }

        /// <summary>
        /// The outcome, result, of this test session.
        /// </summary>
        public TestOutcome TestOutcome { get; }

        /// <summary>
        /// Number of failed tests.
        /// </summary>
        public int FailedTests { get; }

        /// <summary>
        /// Number of skipped tests.
        /// </summary>
        public int Skipped { get; }

        /// <summary>
        /// Number of passed tests.
        /// </summary>
        public int Passed { get; }

        /// <summary>
        /// Raw dump of the complete test session.
        /// When using NUnit this will be XML.
        /// </summary>
        public string ResultDump { get; }

        public TestSessionEnd(int testCount, in DateTime endTime, TestOutcome testOutcome, in int failedTests,
            in int skipped,
            in int passed, string resultDump)
        {
            TestCount = testCount;
            EndTime = endTime;
            TestOutcome = testOutcome;
            FailedTests = failedTests;
            Skipped = skipped;
            Passed = passed;
            ResultDump = resultDump;
        }
    }
}