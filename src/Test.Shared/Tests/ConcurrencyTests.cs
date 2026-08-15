namespace Test.Shared.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using StatusIcons;

    /// <summary>
    /// Tests that exercise the concurrent-dictionary backing under parallel access.
    /// </summary>
    public static class ConcurrencyTests
    {
        private static ConcurrentDictionary<string, string> Active(StatusIcon icon)
        {
            return icon.SupportsUnicode ? icon.UnicodeIcons : icon.AsciiIcons;
        }

        /// <summary>
        /// Execute all concurrency tests through the supplied runner.
        /// </summary>
        /// <param name="runner">Test runner.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task.</returns>
        public static async Task RunAllAsync(TestRunner runner, CancellationToken token = default)
        {
            await runner.RunTestAsync("Parallel writes of distinct keys all land", ct =>
            {
                StatusIcon icon = new StatusIcon();
                ConcurrentDictionary<string, string> active = Active(icon);
                int baseline = active.Count;

                Parallel.For(0, 500, i =>
                {
                    icon["Key" + i] = "V" + i;
                });

                AssertHelper.HasCount(active, baseline + 500, "active set after parallel writes");
                for (int i = 0; i < 500; i++)
                {
                    AssertHelper.AreEqual("V" + i, icon["Key" + i], "Key" + i);
                }
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Concurrent reads of a default key never throw", ct =>
            {
                StatusIcon icon = new StatusIcon();
                Parallel.For(0, 1000, i =>
                {
                    string value = icon["Success"];
                    if (string.IsNullOrEmpty(value))
                        throw new Exception("Concurrent read returned empty value.");
                });
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Concurrent overwrites converge to a valid value", ct =>
            {
                StatusIcon icon = new StatusIcon();
                Parallel.For(0, 500, i =>
                {
                    icon["Contended"] = "V" + (i % 10);
                });

                string final = icon["Contended"];
                AssertHelper.StringContains(final, "V", "final contended value");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Mixed concurrent reads and writes remain consistent", ct =>
            {
                StatusIcon icon = new StatusIcon();
                Parallel.For(0, 500, i =>
                {
                    if (i % 2 == 0) icon["Mixed" + i] = "M" + i;
                    else { string _ = icon["Success"]; }
                });

                for (int i = 0; i < 500; i += 2)
                {
                    AssertHelper.AreEqual("M" + i, icon["Mixed" + i], "Mixed" + i);
                }
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);
        }
    }
}
