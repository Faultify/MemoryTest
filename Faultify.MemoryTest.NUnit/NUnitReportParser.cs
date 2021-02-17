using System;
using System.IO;
using System.Linq;
using System.Xml;
using Faultify.MemoryTest.TestInformation;

namespace Faultify.MemoryTest.NUnit
{
    public class NUnitReportParser
    {
        private readonly string _report;
        private readonly XmlDocument _document;
        private readonly XmlAttributeCollection _attributes;

        public NUnitReportParser(string report)
        {
            _report = report;
            _document = new XmlDocument();
            _document.LoadXml(report);
            _attributes = _document.FirstChild.Attributes;
        }

        public bool IsTestSessionStart => _report.Contains("start-run");
        public bool IsTestSessionEnd => _report.Contains("test-run");
        public bool IsTestStart => _report.Contains("start-test");
        public bool IsTestEnd => _report.Contains("test-case");

        public TestStart TestStartInfo()
        {
            var fullTestName = _attributes.GetNamedItem("fullname").Value;
            var fullClassName = _attributes.GetNamedItem("classname").Value;
            var methodName = _attributes.GetNamedItem("methodname").Value;
            var typeName = _attributes.GetNamedItem("type").Value;

            return new TestStart(methodName, typeName, fullTestName, fullClassName);
        }

        public TestEnd TestEndInfo()
        {
            var fullTestName = _attributes.GetNamedItem("fullname").Value;
            var fullClassName = _attributes.GetNamedItem("classname").Value;
            var methodName = _attributes.GetNamedItem("methodname").Value;

            return new TestEnd(methodName, fullClassName.Split('.').Last(), fullTestName, fullClassName, Result(), StartTime(), EndTime());
        }

        public TestSessionEnd TestSessionEnd()
        {
            var testCaseCount = int.Parse(_attributes.GetNamedItem("testcasecount").Value);
            var failed = int.Parse(_attributes.GetNamedItem("failed").Value);
            var passed = int.Parse(_attributes.GetNamedItem("passed").Value);
            var skipped = int.Parse(_attributes.GetNamedItem("skipped").Value);
            
            using var stringWriter = new StringWriter();
            using var xmlTextWriter = XmlWriter.Create(stringWriter);

            _document.WriteTo(xmlTextWriter);
            xmlTextWriter.Flush();

            return new TestSessionEnd(testCaseCount, EndTime(), Result(), failed, skipped, passed,
                stringWriter.GetStringBuilder().ToString());
        }

        public DateTime EndTime()
        {
            return DateTime.Parse(_attributes.GetNamedItem("end-time").Value);
        }

        public DateTime StartTime()
        {
            return DateTime.Parse(_attributes.GetNamedItem("start-time").Value);
        }

        public TestOutcome Result()
        {
            return ParseTestOutcome(_attributes.GetNamedItem("result").Value);
        }

        private TestOutcome ParseTestOutcome(string testOutcome)
        {
            return testOutcome switch
            {
                "Passed" => TestOutcome.Passed,
                "Failed" => TestOutcome.Failed,
                "Skipped" => TestOutcome.Skipped,
                _ => TestOutcome.Skipped
            };
        }
    }
}