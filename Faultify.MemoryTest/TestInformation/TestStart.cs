namespace Faultify.MemoryTest.TestInformation
{
    public readonly struct TestStart : ITestInfo
    {
        public TestStart(string testName, string typeName, string fullTestName, string fullTypeName)
        {
            TestName = testName;
            TypeName = typeName;
            FullTestName = fullTestName;
            FullTypeName = fullTypeName;
        }

        public string TestName { get; }
        public string TypeName { get; }
        public string FullTestName { get; }
        public string FullTypeName { get; }
    }
}