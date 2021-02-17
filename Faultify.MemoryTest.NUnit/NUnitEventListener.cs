using Faultify.MemoryTest.TestInformation;
using NUnit.Engine;
using NUnit.Engine.Extensibility;

namespace Faultify.MemoryTest.NUnit
{
    [Extension(EngineVersion = "3.4")]
    class NUnitEventListener : ITestEventListener
    {
        private readonly TestHostRunner _testHostRunner;

        public NUnitEventListener(TestHostRunner testHostRunner)
        {
            _testHostRunner = testHostRunner;
        }

        public void OnTestEvent(string report)
        {
            var reportParser = new NUnitReportParser(report);   
            
            if (reportParser.IsTestSessionStart)
            {
                _testHostRunner.OnTestSessionStart(new TestSessionStart(reportParser.StartTime()));
            }
            else if (reportParser.IsTestSessionEnd)
            {
                _testHostRunner.OnTestSessionEnd(reportParser.TestSessionEnd());
            }
            else if (reportParser.IsTestStart)
            {
                _testHostRunner.OnTestCaseStart(reportParser.TestStartInfo());
            }
            else if (reportParser.IsTestEnd)
            {
                _testHostRunner.OnTestCaseEnd(reportParser.TestEndInfo());
            }
        }
    }
}