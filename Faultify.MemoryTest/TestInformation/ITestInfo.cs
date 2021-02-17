namespace Faultify.MemoryTest.TestInformation
{
    public interface ITestInfo
    {
        public string TestName { get; }
        public string FullTestName { get; }
        public string TypeName { get; }
        public string FullTypeName { get; }
    }
}