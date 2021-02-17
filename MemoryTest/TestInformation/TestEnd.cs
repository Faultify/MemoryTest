using System;

namespace Faultify.MemoryTest.TestInformation
{
    /// <summary>
    /// Information about a test that finished running.
    /// </summary>
    public readonly struct TestEnd : ITestInfo
    {
        public TestEnd(string testName, string typeName, string fullTestName, string fullTypeName,
            TestOutcome testOutcome, DateTime startTime, DateTime endTime)
        {
            TestName = testName;
            TypeName = typeName;
            FullTestName = fullTestName;
            FullTypeName = fullTypeName;
            TestOutcome = testOutcome;
            StartTime = startTime;
            EndTime = endTime;
        }

        public string TestName { get; }
        public string TypeName { get; }
        public string FullTestName { get; }
        public string FullTypeName { get; }

        /// <summary>
        /// The outcome, test result, of this test.
        /// </summary>
        public TestOutcome TestOutcome { get; }

        /// <summary>
        /// Time of test start.
        /// </summary>
        public DateTime StartTime { get; }

        /// <summary>
        /// Time of test end.
        /// </summary>
        public DateTime EndTime { get; }
    }
}