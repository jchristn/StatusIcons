namespace Test.Shared.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using StatusIcons;

    /// <summary>
    /// Negative-path tests: null arguments, unknown keys, and removed keys.
    /// </summary>
    public static class NegativeTests
    {
        /// <summary>
        /// Execute all negative tests through the supplied runner.
        /// </summary>
        /// <param name="runner">Test runner.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task.</returns>
        public static async Task RunAllAsync(TestRunner runner, CancellationToken token = default)
        {
            await runner.RunTestAsync("Setting AsciiIcons to null throws ArgumentNullException", ct =>
            {
                StatusIcon icon = new StatusIcon();
                AssertHelper.Throws<ArgumentNullException>(() => icon.AsciiIcons = null, "AsciiIcons = null");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Setting UnicodeIcons to null throws ArgumentNullException", ct =>
            {
                StatusIcon icon = new StatusIcon();
                AssertHelper.Throws<ArgumentNullException>(() => icon.UnicodeIcons = null, "UnicodeIcons = null");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("AsciiIcons remains unchanged after a rejected null assignment", ct =>
            {
                StatusIcon icon = new StatusIcon();
                AssertHelper.Throws<ArgumentNullException>(() => icon.AsciiIcons = null, "AsciiIcons = null");
                AssertHelper.HasCount(icon.AsciiIcons, 7, "AsciiIcons after rejected null");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("UnicodeIcons remains unchanged after a rejected null assignment", ct =>
            {
                StatusIcon icon = new StatusIcon();
                AssertHelper.Throws<ArgumentNullException>(() => icon.UnicodeIcons = null, "UnicodeIcons = null");
                AssertHelper.HasCount(icon.UnicodeIcons, 7, "UnicodeIcons after rejected null");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Indexer get on an unknown key throws KeyNotFoundException", ct =>
            {
                StatusIcon icon = new StatusIcon();
                AssertHelper.Throws<KeyNotFoundException>(() => { string _ = icon["DoesNotExist"]; }, "icon[DoesNotExist]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("AsciiIcons get on an unknown key throws KeyNotFoundException", ct =>
            {
                StatusIcon icon = new StatusIcon();
                AssertHelper.Throws<KeyNotFoundException>(() => { string _ = icon.AsciiIcons["DoesNotExist"]; }, "AsciiIcons[DoesNotExist]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("UnicodeIcons get on an unknown key throws KeyNotFoundException", ct =>
            {
                StatusIcon icon = new StatusIcon();
                AssertHelper.Throws<KeyNotFoundException>(() => { string _ = icon.UnicodeIcons["DoesNotExist"]; }, "UnicodeIcons[DoesNotExist]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Indexer get after a key is removed throws KeyNotFoundException", ct =>
            {
                StatusIcon icon = new StatusIcon();
                icon["Temp"] = "T";
                string _removed;
                icon.Icons.TryRemove("Temp", out _removed);
                AssertHelper.Throws<KeyNotFoundException>(() => { string _ = icon["Temp"]; }, "icon[Temp] after removal");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Indexer get with a null key throws ArgumentNullException", ct =>
            {
                StatusIcon icon = new StatusIcon();
                AssertHelper.Throws<ArgumentNullException>(() => { string _ = icon[null]; }, "icon[null] get");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Indexer set with a null key throws ArgumentNullException", ct =>
            {
                StatusIcon icon = new StatusIcon();
                AssertHelper.Throws<ArgumentNullException>(() => { icon[null] = "x"; }, "icon[null] set");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);
        }
    }
}
