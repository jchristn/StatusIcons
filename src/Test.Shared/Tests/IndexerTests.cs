namespace Test.Shared.Tests
{
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using StatusIcons;

    /// <summary>
    /// Tests for the string indexer get/set behavior.
    /// </summary>
    public static class IndexerTests
    {
        private static ConcurrentDictionary<string, string> Active(StatusIcon icon)
        {
            return icon.SupportsUnicode ? icon.UnicodeIcons : icon.AsciiIcons;
        }

        private static ConcurrentDictionary<string, string> Inactive(StatusIcon icon)
        {
            return icon.SupportsUnicode ? icon.AsciiIcons : icon.UnicodeIcons;
        }

        /// <summary>
        /// Execute all indexer tests through the supplied runner.
        /// </summary>
        /// <param name="runner">Test runner.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task.</returns>
        public static async Task RunAllAsync(TestRunner runner, CancellationToken token = default)
        {
            await runner.RunTestAsync("Indexer get returns value from the active set", ct =>
            {
                StatusIcon icon = new StatusIcon();
                AssertHelper.AreEqual(Active(icon)["Success"], icon["Success"], "indexer[Success]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Indexer get matches active set for all default keys", ct =>
            {
                StatusIcon icon = new StatusIcon();
                foreach (string key in new[] { "Success", "Error", "Warning", "Info", "Working", "Bullet", "Arrow" })
                {
                    AssertHelper.AreEqual(Active(icon)[key], icon[key], "indexer[" + key + "]");
                }
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Indexer set then get round-trips", ct =>
            {
                StatusIcon icon = new StatusIcon();
                icon["Custom"] = "@";
                AssertHelper.AreEqual("@", icon["Custom"], "indexer[Custom]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Indexer set writes only to the active set", ct =>
            {
                StatusIcon icon = new StatusIcon();
                icon["Custom"] = "@";
                AssertHelper.IsTrue(Active(icon).ContainsKey("Custom"), "active set should contain Custom");
                AssertHelper.IsFalse(Inactive(icon).ContainsKey("Custom"), "inactive set should not contain Custom");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Indexer set overwrites an existing value", ct =>
            {
                StatusIcon icon = new StatusIcon();
                icon["Success"] = "OK";
                AssertHelper.AreEqual("OK", icon["Success"], "indexer[Success] after overwrite");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Indexer set accepts empty string value", ct =>
            {
                StatusIcon icon = new StatusIcon();
                icon["Blank"] = string.Empty;
                AssertHelper.AreEqual(string.Empty, icon["Blank"], "indexer[Blank]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Indexer get reflects mutation made through the active dictionary", ct =>
            {
                StatusIcon icon = new StatusIcon();
                Active(icon)["Direct"] = "D";
                AssertHelper.AreEqual("D", icon["Direct"], "indexer[Direct]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Indexer set is visible through the Icons property", ct =>
            {
                StatusIcon icon = new StatusIcon();
                icon["Shared"] = "S";
                AssertHelper.IsTrue(icon.Icons.ContainsKey("Shared"), "Icons should contain Shared");
                AssertHelper.AreEqual("S", icon.Icons["Shared"], "Icons[Shared]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Indexer set with forceUnicode true writes only to the Unicode set", ct =>
            {
                StatusIcon icon = new StatusIcon(true);
                icon["Custom"] = "@";
                AssertHelper.IsTrue(icon.UnicodeIcons.ContainsKey("Custom"), "UnicodeIcons should contain Custom");
                AssertHelper.IsFalse(icon.AsciiIcons.ContainsKey("Custom"), "AsciiIcons should not contain Custom");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Indexer round-trips a multi-codepoint emoji value", ct =>
            {
                StatusIcon icon = new StatusIcon();
                icon["Speaker"] = "🔊";
                AssertHelper.AreEqual("🔊", icon["Speaker"], "indexer[Speaker]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);

            await runner.RunTestAsync("Indexer set with a null value stores and returns null", ct =>
            {
                StatusIcon icon = new StatusIcon();
                icon["Nullable"] = null;
                AssertHelper.IsNull(icon["Nullable"], "indexer[Nullable]");
                return Task.CompletedTask;
            }, token).ConfigureAwait(false);
        }
    }
}
