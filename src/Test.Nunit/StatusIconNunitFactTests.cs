namespace Test.Nunit
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Test.Shared;
    using Touchstone.Core;
    using Touchstone.NunitAdapter;

    /// <summary>
    /// NUnit fact-style host that runs every shared StatusIcon descriptor as a single test.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class StatusIconNunitFactTests : TouchstoneNunitBase
    {
        /// <inheritdoc />
        protected override IReadOnlyList<TestSuiteDescriptor> Suites
        {
            get { return StatusIconSuites.All; }
        }

        /// <summary>
        /// Run all shared descriptors as one NUnit test.
        /// </summary>
        /// <returns>Task.</returns>
        [Test]
        public async Task RunAll()
        {
            await RunAllAsync().ConfigureAwait(false);
        }
    }
}
