namespace Faultify.MemoryTest.TestInformation
{
    /// <summary>
    /// Interface for data describing a test. 
    /// </summary>
    public interface ITestInfo
    {
        /// <summary>
        /// The name of the test.
        /// </summary>
        public string TestName { get; }

        /// <summary>
        /// The full name, with name space, of the test.
        /// </summary>
        public string FullTestName { get; }

        /// <summary>
        /// The name of the type in which the test method exist.
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// The name of the type, with name space, in which the test method exist.
        /// </summary>
        public string FullTypeName { get; }
    }
}