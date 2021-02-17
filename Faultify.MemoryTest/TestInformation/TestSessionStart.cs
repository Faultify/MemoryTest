using System;

namespace Faultify.MemoryTest.TestInformation
{
    public readonly struct TestSessionStart : ITestSessionInfo
    {
        public DateTime StartTime { get; }

        public TestSessionStart(DateTime startTime)
        {
            StartTime = startTime;
        }
    }
}