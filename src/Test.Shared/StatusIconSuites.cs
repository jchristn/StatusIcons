namespace Test.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Test.Shared.Tests;
    using Touchstone.Core;

    /// <summary>
    /// Shared Touchstone suite descriptors for StatusIcons.
    /// This is the single source of truth exercised by the console runner (Test.Automated),
    /// the xUnit adapter (Test.Xunit), and the NUnit adapter (Test.Nunit).
    /// </summary>
    public static class StatusIconSuites
    {
        /// <summary>
        /// All shared test suites.
        /// </summary>
        public static IReadOnlyList<TestSuiteDescriptor> All
        {
            get
            {
                return new List<TestSuiteDescriptor>
                {
                    CoreSuite(),
                    BehaviorSuite(),
                    NegativeSuite()
                };
            }
        }

        /// <summary>
        /// Core construction, default-icon, indexer, and property behavior.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor CoreSuite()
        {
            const string suiteId = "Core";

            return new TestSuiteDescriptor(
                suiteId: suiteId,
                displayName: "StatusIcon Core",
                cases: BuildRunnerCases(
                    suiteId,
                    new RunnerSource("Constructor", "Constructor", ConstructorTests.RunAllAsync),
                    new RunnerSource("DefaultIcons", "Default icons", DefaultIconTests.RunAllAsync),
                    new RunnerSource("Indexer", "Indexer", IndexerTests.RunAllAsync),
                    new RunnerSource("Properties", "Properties", PropertyTests.RunAllAsync)));
        }

        /// <summary>
        /// Concurrency and diagnostic (TestTerminal) behavior.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor BehaviorSuite()
        {
            const string suiteId = "Behavior";

            return new TestSuiteDescriptor(
                suiteId: suiteId,
                displayName: "StatusIcon Behavior",
                cases: BuildRunnerCases(
                    suiteId,
                    new RunnerSource("Concurrency", "Concurrency", ConcurrencyTests.RunAllAsync),
                    new RunnerSource("TestTerminal", "Test terminal", TestTerminalTests.RunAllAsync)));
        }

        /// <summary>
        /// Negative-path behavior: null arguments, unknown keys, and removed keys.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor NegativeSuite()
        {
            const string suiteId = "Negative";

            return new TestSuiteDescriptor(
                suiteId: suiteId,
                displayName: "StatusIcon Negative",
                cases: BuildRunnerCases(
                    suiteId,
                    new RunnerSource("Negative", "Negative", NegativeTests.RunAllAsync)));
        }

        #region Bridging

        private static IReadOnlyList<TestCaseDescriptor> BuildRunnerCases(
            string suiteId,
            params RunnerSource[] sources)
        {
            List<TestCaseDescriptor> cases = new List<TestCaseDescriptor>();

            foreach (RunnerSource source in sources)
            {
                foreach (string testName in DiscoverTestNames(source.ExecuteAsync))
                {
                    string caseId = source.CategoryId + "_" + NormalizeCaseId(testName);
                    string displayName = source.DisplayName + ": " + testName;

                    cases.Add(RunnerCase(suiteId, caseId, displayName, source.ExecuteAsync, testName));
                }
            }

            return cases;
        }

        private static IReadOnlyList<string> DiscoverTestNames(
            Func<TestRunner, CancellationToken, Task> executeAsync)
        {
            TestRunner runner = TestRunner.CreateDiscoveryRunner();
            executeAsync(runner, CancellationToken.None).GetAwaiter().GetResult();

            List<string> names = new List<string>();
            HashSet<string> seen = new HashSet<string>(StringComparer.Ordinal);

            foreach (string name in runner.DiscoveredTests)
            {
                if (seen.Add(name))
                    names.Add(name);
            }

            return names;
        }

        private static TestCaseDescriptor RunnerCase(
            string suiteId,
            string caseId,
            string displayName,
            Func<TestRunner, CancellationToken, Task> executeAsync,
            string targetTestName)
        {
            return new TestCaseDescriptor(
                suiteId: suiteId,
                caseId: caseId,
                displayName: displayName,
                executeAsync: async ct =>
                {
                    TestRunner runner = TestRunner.CreateTargetedRunner(targetTestName);
                    await executeAsync(runner, ct).ConfigureAwait(false);

                    if (!runner.TargetWasExecuted)
                        throw new InvalidOperationException("Target test was not executed: " + targetTestName);

                    AssertAllPassed(runner);
                });
        }

        private static void AssertAllPassed(TestRunner runner)
        {
            StringBuilder failures = new StringBuilder();

            foreach (TestResult result in runner.Results)
            {
                if (result.Passed) continue;
                failures.AppendLine(result.TestName + ": " + result.ErrorMessage);
            }

            if (failures.Length > 0)
                throw new InvalidOperationException(failures.ToString());
        }

        private static string NormalizeCaseId(string value)
        {
            if (String.IsNullOrEmpty(value)) return "Unnamed";

            StringBuilder builder = new StringBuilder();
            bool capitalizeNext = true;

            foreach (char c in value)
            {
                if (Char.IsLetterOrDigit(c))
                {
                    if (builder.Length == 0 && Char.IsDigit(c))
                        builder.Append("Case");

                    builder.Append(capitalizeNext ? Char.ToUpperInvariant(c) : c);
                    capitalizeNext = false;
                }
                else
                {
                    capitalizeNext = true;
                }
            }

            if (builder.Length == 0)
                return "Unnamed";

            return builder.ToString();
        }

        private sealed class RunnerSource
        {
            public string CategoryId { get; private set; }
            public string DisplayName { get; private set; }
            public Func<TestRunner, CancellationToken, Task> ExecuteAsync { get; private set; }

            public RunnerSource(
                string categoryId,
                string displayName,
                Func<TestRunner, CancellationToken, Task> executeAsync)
            {
                if (String.IsNullOrEmpty(categoryId)) throw new ArgumentNullException(nameof(categoryId));
                if (String.IsNullOrEmpty(displayName)) throw new ArgumentNullException(nameof(displayName));
                if (executeAsync == null) throw new ArgumentNullException(nameof(executeAsync));

                CategoryId = categoryId;
                DisplayName = displayName;
                ExecuteAsync = executeAsync;
            }
        }

        #endregion
    }
}
