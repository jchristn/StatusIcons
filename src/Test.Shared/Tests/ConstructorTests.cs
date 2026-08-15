namespace Test.Shared.Tests
{
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using StatusIcons;

    /// <summary>
    /// Tests for StatusIcon construction and unicode-support detection.
    /// </summary>
    public static class ConstructorTests
    {
        /// <summary>
        /// Execute all constructor tests through the supplied runner.
        /// </summary>
        /// <param name="runner">Test runner.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task.</returns>
        public static async Task RunAllAsync(TestRunner runner, CancellationToken token = default)
        {
            await runner.RunTestAsync("Default constructor does not throw", ct =>
            {
                StatusIcon icon = new StatusIcon();
                AssertHelper.IsNotNull(icon, "icon");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Explicit forceUnicode false does not throw", ct =>
            {
                StatusIcon icon = new StatusIcon(false);
                AssertHelper.IsNotNull(icon, "icon");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("forceUnicode true sets SupportsUnicode true", ct =>
            {
                StatusIcon icon = new StatusIcon(true);
                AssertHelper.IsTrue(icon.SupportsUnicode, "SupportsUnicode should be true when forced");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("forceUnicode true exposes unicode set via Icons", ct =>
            {
                StatusIcon icon = new StatusIcon(true);
                AssertHelper.AreSame(icon.UnicodeIcons, icon.Icons, "Icons");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("forceUnicode true indexer returns unicode value", ct =>
            {
                StatusIcon icon = new StatusIcon(true);
                AssertHelper.AreEqual("✓", icon["Success"], "Success (unicode)");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("SupportsUnicode matches active Icons set", ct =>
            {
                StatusIcon icon = new StatusIcon();
                ConcurrentDictionary<string, string> expected = icon.SupportsUnicode
                    ? icon.UnicodeIcons
                    : icon.AsciiIcons;
                AssertHelper.AreSame(expected, icon.Icons, "Icons");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Distinct instances are independent", ct =>
            {
                StatusIcon a = new StatusIcon();
                StatusIcon b = new StatusIcon();

                a.AsciiIcons["Isolated"] = "@";

                AssertHelper.IsTrue(a.AsciiIcons.ContainsKey("Isolated"), "instance a should have new key");
                AssertHelper.IsFalse(b.AsciiIcons.ContainsKey("Isolated"), "instance b should not have new key");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Backing dictionaries are distinct instances", ct =>
            {
                StatusIcon icon = new StatusIcon();
                AssertHelper.AreNotSame(icon.AsciiIcons, icon.UnicodeIcons, "Ascii vs Unicode dictionaries");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);
        }
    }
}
