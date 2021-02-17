using System;

namespace Faultify.MemoryTest.TestInformation
{
    /// <summary>
    /// Information about a test session that started running.
    /// </summary>
    public readonly struct TestSessionStart : ITestSessionInfo
    {
        /// <summary>
        /// Time of the test session start. 
        /// </summary>
        public DateTime StartTime { get; }

        public TestSessionStart(DateTime startTime)
        {
            StartTime = startTime;
        }
    }
}