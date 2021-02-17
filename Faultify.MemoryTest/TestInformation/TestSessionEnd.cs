using System;

namespace Faultify.MemoryTest.TestInformation
{
    public readonly struct TestSessionEnd : ITestSessionInfo
    {
        public int TestCaseCount { get; }
        public DateTime EndTime { get; }
        public TestOutcome TestOutcome { get; }
        public int Failed { get; }
        public int Skipped { get; }
        public int Passed { get; }

        public string ResultDump { get; }

        public TestSessionEnd(int testCaseCount, in DateTime endTime, TestOutcome testOutcome, in int failed,
            in int skipped,
            in int passed, string resultDump)
        {
            TestCaseCount = testCaseCount;
            EndTime = endTime;
            TestOutcome = testOutcome;
            Failed = failed;
            Skipped = skipped;
            Passed = passed;
            ResultDump = resultDump;
        }
    }
}