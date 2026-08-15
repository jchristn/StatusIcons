namespace Test.Shared.Tests
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using StatusIcons;

    /// <summary>
    /// Tests for the TestTerminal diagnostic method.
    /// Console output is redirected during these tests to keep runner output clean.
    /// </summary>
    public static class TestTerminalTests
    {
        private static string CaptureTestTerminal(StatusIcon icon)
        {
            TextWriter original = Console.Out;
            StringWriter buffer = new StringWriter();
            try
            {
                Console.SetOut(buffer);
                icon.TestTerminal();
            }
            finally
            {
                Console.SetOut(original);
            }
            return buffer.ToString();
        }

        /// <summary>
        /// Execute all TestTerminal tests through the supplied runner.
        /// </summary>
        /// <param name="runner">Test runner.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task.</returns>
        public static async Task RunAllAsync(TestRunner runner, CancellationToken token = default)
        {
            await runner.RunTestAsync("TestTerminal with default constructor does not throw", ct =>
            {
                CaptureTestTerminal(new StatusIcon());
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("TestTerminal with forceUnicode does not throw", ct =>
            {
                CaptureTestTerminal(new StatusIcon(true));
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("TestTerminal output includes the capability header", ct =>
            {
                string output = CaptureTestTerminal(new StatusIcon());
                AssertHelper.StringContains(output, "Terminal Capability Test", "TestTerminal output");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("TestTerminal output enumerates the current icons", ct =>
            {
                string output = CaptureTestTerminal(new StatusIcon());
                AssertHelper.StringContains(output, "Current Icons", "TestTerminal output");
                AssertHelper.StringContains(output, "Success", "TestTerminal output");
                AssertHelper.StringContains(output, "Arrow", "TestTerminal output");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("TestTerminal reports the detected unicode-support flag", ct =>
            {
                StatusIcon icon = new StatusIcon();
                string output = CaptureTestTerminal(icon);
                AssertHelper.StringContains(output, "Unicode Support Detected", "TestTerminal output");
                AssertHelper.StringContains(output, icon.SupportsUnicode.ToString(), "TestTerminal output support flag");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);
        }
    }
}
