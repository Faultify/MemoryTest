namespace Faultify.MemoryTest
{
    /// <summary>
    /// The outcome of a test.
    /// </summary>
    public enum TestOutcome
    {
        /// <summary>
        /// A test passed successfully without errors. 
        /// </summary>
        Passed,
        /// <summary>
        /// A test failed with errors.
        /// </summary>
        Failed,
        /// <summary>
        /// A test skipped running.
        /// </summary>
        Skipped
    }
}