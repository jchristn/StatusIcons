namespace Test.Shared.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using StatusIcons;

    /// <summary>
    /// Tests for the default ASCII and Unicode icon sets.
    /// </summary>
    public static class DefaultIconTests
    {
        private static readonly string[] _DefaultKeys =
            { "Success", "Error", "Warning", "Info", "Working", "Bullet", "Arrow" };

        /// <summary>
        /// Execute all default-icon tests through the supplied runner.
        /// </summary>
        /// <param name="runner">Test runner.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task.</returns>
        public static async Task RunAllAsync(TestRunner runner, CancellationToken token = default)
        {
            await runner.RunTestAsync("Default ASCII Success is plus", ct =>
            {
                AssertHelper.AreEqual("+", new StatusIcon().AsciiIcons["Success"], "AsciiIcons[Success]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Default ASCII Error is X", ct =>
            {
                AssertHelper.AreEqual("X", new StatusIcon().AsciiIcons["Error"], "AsciiIcons[Error]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Default ASCII Warning is bang", ct =>
            {
                AssertHelper.AreEqual("!", new StatusIcon().AsciiIcons["Warning"], "AsciiIcons[Warning]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Default ASCII Info is lowercase i", ct =>
            {
                AssertHelper.AreEqual("i", new StatusIcon().AsciiIcons["Info"], "AsciiIcons[Info]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Default ASCII Working is dot", ct =>
            {
                AssertHelper.AreEqual(".", new StatusIcon().AsciiIcons["Working"], "AsciiIcons[Working]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Default ASCII Bullet is asterisk", ct =>
            {
                AssertHelper.AreEqual("*", new StatusIcon().AsciiIcons["Bullet"], "AsciiIcons[Bullet]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Default ASCII Arrow is greater-than", ct =>
            {
                AssertHelper.AreEqual(">", new StatusIcon().AsciiIcons["Arrow"], "AsciiIcons[Arrow]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Default Unicode Success is check mark", ct =>
            {
                AssertHelper.AreEqual("✓", new StatusIcon().UnicodeIcons["Success"], "UnicodeIcons[Success]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Default Unicode Error is ballot X", ct =>
            {
                AssertHelper.AreEqual("✗", new StatusIcon().UnicodeIcons["Error"], "UnicodeIcons[Error]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Default Unicode Warning is warning sign", ct =>
            {
                AssertHelper.AreEqual("⚠", new StatusIcon().UnicodeIcons["Warning"], "UnicodeIcons[Warning]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Default Unicode Info is information source", ct =>
            {
                AssertHelper.AreEqual("ℹ", new StatusIcon().UnicodeIcons["Info"], "UnicodeIcons[Info]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Default Unicode Working is midline ellipsis", ct =>
            {
                AssertHelper.AreEqual("⋯", new StatusIcon().UnicodeIcons["Working"], "UnicodeIcons[Working]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Default Unicode Bullet is bullet", ct =>
            {
                AssertHelper.AreEqual("•", new StatusIcon().UnicodeIcons["Bullet"], "UnicodeIcons[Bullet]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Default Unicode Arrow is rightwards arrow", ct =>
            {
                AssertHelper.AreEqual("→", new StatusIcon().UnicodeIcons["Arrow"], "UnicodeIcons[Arrow]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("ASCII set contains exactly seven defaults", ct =>
            {
                AssertHelper.HasCount(new StatusIcon().AsciiIcons, 7, "AsciiIcons");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Unicode set contains exactly seven defaults", ct =>
            {
                AssertHelper.HasCount(new StatusIcon().UnicodeIcons, 7, "UnicodeIcons");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Both sets contain the same default keys", ct =>
            {
                StatusIcon icon = new StatusIcon();
                foreach (string key in _DefaultKeys)
                {
                    AssertHelper.IsTrue(icon.AsciiIcons.ContainsKey(key), "AsciiIcons should contain " + key);
                    AssertHelper.IsTrue(icon.UnicodeIcons.ContainsKey(key), "UnicodeIcons should contain " + key);
                }
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Unicode and ASCII values differ for every default key", ct =>
            {
                StatusIcon icon = new StatusIcon();
                foreach (string key in _DefaultKeys)
                {
                    AssertHelper.AreNotEqual(icon.AsciiIcons[key], icon.UnicodeIcons[key], "value for " + key);
                }
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("No default icon value is null or empty", ct =>
            {
                StatusIcon icon = new StatusIcon();
                foreach (string key in _DefaultKeys)
                {
                    AssertHelper.IsFalse(string.IsNullOrEmpty(icon.AsciiIcons[key]), "AsciiIcons[" + key + "] non-empty");
                    AssertHelper.IsFalse(string.IsNullOrEmpty(icon.UnicodeIcons[key]), "UnicodeIcons[" + key + "] non-empty");
                }
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);
        }
    }
}
