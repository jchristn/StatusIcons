namespace StatusIcons.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using StatusIcons;
    using Xunit;

    public class StatusIconTests
    {
        private static readonly string[] _DefaultKeys =
            { "Success", "Error", "Warning", "Info", "Working", "Bullet", "Arrow" };

        #region Positive-Cases

        [Fact]
        public void Constructor_PopulatesDefaultAsciiIcons()
        {
            StatusIcon icon = new StatusIcon();

            Assert.Equal("+", icon.AsciiIcons["Success"]);
            Assert.Equal("X", icon.AsciiIcons["Error"]);
            Assert.Equal("!", icon.AsciiIcons["Warning"]);
            Assert.Equal("i", icon.AsciiIcons["Info"]);
            Assert.Equal(".", icon.AsciiIcons["Working"]);
            Assert.Equal("*", icon.AsciiIcons["Bullet"]);
            Assert.Equal(">", icon.AsciiIcons["Arrow"]);
        }

        [Fact]
        public void Constructor_PopulatesDefaultUnicodeIcons()
        {
            StatusIcon icon = new StatusIcon();

            Assert.Equal("✓", icon.UnicodeIcons["Success"]); // ✓
            Assert.Equal("✗", icon.UnicodeIcons["Error"]);   // ✗
            Assert.Equal("⚠", icon.UnicodeIcons["Warning"]); // ⚠
            Assert.Equal("ℹ", icon.UnicodeIcons["Info"]);    // ℹ
            Assert.Equal("⋯", icon.UnicodeIcons["Working"]); // ⋯
            Assert.Equal("•", icon.UnicodeIcons["Bullet"]);  // •
            Assert.Equal("→", icon.UnicodeIcons["Arrow"]);   // →
        }

        [Fact]
        public void BothIconSets_ContainSameKeys()
        {
            StatusIcon icon = new StatusIcon();

            foreach (string key in _DefaultKeys)
            {
                Assert.True(icon.AsciiIcons.ContainsKey(key));
                Assert.True(icon.UnicodeIcons.ContainsKey(key));
            }

            Assert.Equal(_DefaultKeys.Length, icon.AsciiIcons.Count);
            Assert.Equal(_DefaultKeys.Length, icon.UnicodeIcons.Count);
        }

        [Fact]
        public void Indexer_Get_ReturnsValueMatchingUnicodeSupport()
        {
            StatusIcon icon = new StatusIcon();

            // The indexer returns from whichever backing set matches the detected
            // support level, so assert against that same set to stay deterministic
            // regardless of the terminal the tests happen to run in.
            string expected = icon.SupportsUnicode
                ? icon.UnicodeIcons["Success"]
                : icon.AsciiIcons["Success"];

            Assert.Equal(expected, icon["Success"]);
        }

        [Fact]
        public void Icons_ReturnsSetMatchingUnicodeSupport()
        {
            StatusIcon icon = new StatusIcon();

            ConcurrentDictionary<string, string> expected = icon.SupportsUnicode
                ? icon.UnicodeIcons
                : icon.AsciiIcons;

            Assert.Same(expected, icon.Icons);
        }

        [Fact]
        public void Indexer_Set_UpdatesActiveSet()
        {
            StatusIcon icon = new StatusIcon();

            icon["Custom"] = "@";

            Assert.Equal("@", icon["Custom"]);

            // Written to the active set only.
            if (icon.SupportsUnicode)
            {
                Assert.True(icon.UnicodeIcons.ContainsKey("Custom"));
                Assert.False(icon.AsciiIcons.ContainsKey("Custom"));
            }
            else
            {
                Assert.True(icon.AsciiIcons.ContainsKey("Custom"));
                Assert.False(icon.UnicodeIcons.ContainsKey("Custom"));
            }
        }

        [Fact]
        public void Indexer_Set_OverwritesExistingValue()
        {
            StatusIcon icon = new StatusIcon();

            icon["Success"] = "OK";

            Assert.Equal("OK", icon["Success"]);
        }

        [Fact]
        public void AsciiIcons_CanBeReplacedWithNewDictionary()
        {
            StatusIcon icon = new StatusIcon();

            ConcurrentDictionary<string, string> replacement =
                new ConcurrentDictionary<string, string>(
                    new Dictionary<string, string> { { "Only", "1" } });

            icon.AsciiIcons = replacement;

            Assert.Same(replacement, icon.AsciiIcons);
            Assert.Equal("1", icon.AsciiIcons["Only"]);
        }

        [Fact]
        public void UnicodeIcons_CanBeReplacedWithNewDictionary()
        {
            StatusIcon icon = new StatusIcon();

            ConcurrentDictionary<string, string> replacement =
                new ConcurrentDictionary<string, string>(
                    new Dictionary<string, string> { { "Only", "2" } });

            icon.UnicodeIcons = replacement;

            Assert.Same(replacement, icon.UnicodeIcons);
            Assert.Equal("2", icon.UnicodeIcons["Only"]);
        }

        #endregion

        #region Negative-Cases

        [Fact]
        public void AsciiIcons_SetNull_Throws()
        {
            StatusIcon icon = new StatusIcon();

            Assert.Throws<ArgumentNullException>(() => icon.AsciiIcons = null);
        }

        [Fact]
        public void UnicodeIcons_SetNull_Throws()
        {
            StatusIcon icon = new StatusIcon();

            Assert.Throws<ArgumentNullException>(() => icon.UnicodeIcons = null);
        }

        [Fact]
        public void Indexer_Get_UnknownKey_Throws()
        {
            StatusIcon icon = new StatusIcon();

            Assert.Throws<KeyNotFoundException>(() => _ = icon["DoesNotExist"]);
        }

        [Fact]
        public void AsciiIcons_UnknownKey_Throws()
        {
            StatusIcon icon = new StatusIcon();

            Assert.Throws<KeyNotFoundException>(() => _ = icon.AsciiIcons["DoesNotExist"]);
        }

        [Fact]
        public void UnicodeIcons_UnknownKey_Throws()
        {
            StatusIcon icon = new StatusIcon();

            Assert.Throws<KeyNotFoundException>(() => _ = icon.UnicodeIcons["DoesNotExist"]);
        }

        #endregion
    }
}
