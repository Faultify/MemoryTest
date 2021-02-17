using System;

namespace Faultify.MemoryTest.TestInformation
{
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
        public TestOutcome TestOutcome { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
    }
}