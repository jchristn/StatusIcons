namespace Test.Shared.Tests
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using StatusIcons;

    /// <summary>
    /// Tests for the Icons, AsciiIcons, and UnicodeIcons properties (getters, setters, replacement).
    /// </summary>
    public static class PropertyTests
    {
        /// <summary>
        /// Execute all property tests through the supplied runner.
        /// </summary>
        /// <param name="runner">Test runner.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task.</returns>
        public static async Task RunAllAsync(TestRunner runner, CancellationToken token = default)
        {
            await runner.RunTestAsync("Icons getter is never null", ct =>
            {
                AssertHelper.IsNotNull(new StatusIcon().Icons, "Icons");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Icons getter returns the active backing set", ct =>
            {
                StatusIcon icon = new StatusIcon();
                ConcurrentDictionary<string, string> expected = icon.SupportsUnicode ? icon.UnicodeIcons : icon.AsciiIcons;
                AssertHelper.AreSame(expected, icon.Icons, "Icons");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Icons getter exposes the seven defaults", ct =>
            {
                AssertHelper.HasCount(new StatusIcon().Icons, 7, "Icons");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("AsciiIcons getter is never null", ct =>
            {
                AssertHelper.IsNotNull(new StatusIcon().AsciiIcons, "AsciiIcons");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("UnicodeIcons getter is never null", ct =>
            {
                AssertHelper.IsNotNull(new StatusIcon().UnicodeIcons, "UnicodeIcons");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("AsciiIcons can be replaced with a new dictionary", ct =>
            {
                StatusIcon icon = new StatusIcon();
                ConcurrentDictionary<string, string> replacement =
                    new ConcurrentDictionary<string, string>(new Dictionary<string, string> { { "Only", "1" } });

                icon.AsciiIcons = replacement;

                AssertHelper.AreSame(replacement, icon.AsciiIcons, "AsciiIcons");
                AssertHelper.AreEqual("1", icon.AsciiIcons["Only"], "AsciiIcons[Only]");
                AssertHelper.HasCount(icon.AsciiIcons, 1, "AsciiIcons after replace");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("UnicodeIcons can be replaced with a new dictionary", ct =>
            {
                StatusIcon icon = new StatusIcon();
                ConcurrentDictionary<string, string> replacement =
                    new ConcurrentDictionary<string, string>(new Dictionary<string, string> { { "Only", "2" } });

                icon.UnicodeIcons = replacement;

                AssertHelper.AreSame(replacement, icon.UnicodeIcons, "UnicodeIcons");
                AssertHelper.AreEqual("2", icon.UnicodeIcons["Only"], "UnicodeIcons[Only]");
                AssertHelper.HasCount(icon.UnicodeIcons, 1, "UnicodeIcons after replace");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Replacing the active set changes what Icons returns", ct =>
            {
                StatusIcon icon = new StatusIcon(true); // active set is Unicode
                ConcurrentDictionary<string, string> replacement =
                    new ConcurrentDictionary<string, string>(new Dictionary<string, string> { { "Success", "U" } });

                icon.UnicodeIcons = replacement;

                AssertHelper.AreSame(replacement, icon.Icons, "Icons after active replace");
                AssertHelper.AreEqual("U", icon["Success"], "indexer[Success] after active replace");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Replacing AsciiIcons leaves UnicodeIcons intact", ct =>
            {
                StatusIcon icon = new StatusIcon();
                icon.AsciiIcons = new ConcurrentDictionary<string, string>(new Dictionary<string, string> { { "Only", "1" } });
                AssertHelper.HasCount(icon.UnicodeIcons, 7, "UnicodeIcons should be untouched");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Direct AsciiIcons mutation is observable", ct =>
            {
                StatusIcon icon = new StatusIcon();
                icon.AsciiIcons["New"] = "N";
                AssertHelper.IsTrue(icon.AsciiIcons.ContainsKey("New"), "AsciiIcons should contain New");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Direct UnicodeIcons mutation is observable", ct =>
            {
                StatusIcon icon = new StatusIcon();
                icon.UnicodeIcons["New"] = "N";
                AssertHelper.IsTrue(icon.UnicodeIcons.ContainsKey("New"), "UnicodeIcons should contain New");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);
        }
    }
}
