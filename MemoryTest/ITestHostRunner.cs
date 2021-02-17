using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Faultify.MemoryTest
{
    /// <summary>
    ///     Interface for running tests and code coverage on some test host.
    /// </summary>
    public interface ITestHostRunner
    {
        /// <summary>
        ///     Runs the given tests asynchronously.
        ///     If no tests are passed then all tests will be run.
        /// </summary>
        Task RunTestsAsync(CancellationToken token, HashSet<string> tests = null);
    }
}