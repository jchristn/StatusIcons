namespace StatusIcons
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Status icon.
    /// </summary>
    public class StatusIcon
    {
        #region Public-Members

        /// <summary>
        /// Access a specific icon.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Value.</returns>
        public string this[string key]
        {
            get
            {
                if (_SupportsUnicode) return _UnicodeIcons[key];
                else return _AsciiIcons[key];
            }
            set
            {
                if (_SupportsUnicode) _UnicodeIcons[key] = value;
                else _AsciiIcons[key] = value;
            }
        }

        /// <summary>
        /// Retrieve the backing dictionary.
        /// </summary>
        public ConcurrentDictionary<string, string> Icons
        {
            get
            {
                if (_SupportsUnicode) return _UnicodeIcons;
                else return _AsciiIcons;
            }
        }

        /// <summary>
        /// ASCII icons.
        /// </summary>
        public ConcurrentDictionary<string, string> AsciiIcons
        {
            get
            {
                return _AsciiIcons;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(AsciiIcons));
                _AsciiIcons = value;
            }
        }

        /// <summary>
        /// Unicode icons.
        /// </summary>
        public ConcurrentDictionary<string, string> UnicodeIcons
        {
            get
            {
                return _UnicodeIcons;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(UnicodeIcons));
                _UnicodeIcons = value;
            }
        }

        /// <summary>
        /// Check if Unicode is supported.
        /// </summary>
        public bool SupportsUnicode => _SupportsUnicode;

        #endregion

        #region Private-Members

        private bool _SupportsUnicode = false;
        private ConcurrentDictionary<string, string> _AsciiIcons = new ConcurrentDictionary<string, string>(
            new Dictionary<string, string> {
                { "Success", "+" },
                { "Error", "X" },
                { "Warning", "!" },
                { "Info", "i" },
                { "Working", "." },
                { "Bullet", "*" },
                { "Arrow", ">" }
            }
        );
        private ConcurrentDictionary<string, string> _UnicodeIcons = new ConcurrentDictionary<string, string>(
            new Dictionary<string, string> {
                { "Success", "✓" },
                { "Error", "✗" },
                { "Warning", "⚠" },
                { "Info", "ℹ" },
                { "Working", "⋯" },
                { "Bullet", "•" },
                { "Arrow", "→" }
            }
        );

        #endregion

        #region Constructors-and-Factories

        /// <summary>
        /// Instantiate.
        /// </summary>
        public StatusIcon(bool forceUnicode = false)
        {
            if (forceUnicode)
            {
                _SupportsUnicode = true;
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                return;
            }

            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                _SupportsUnicode = DetectUnicodeSupport();
            }
            catch
            {
                _SupportsUnicode = false;
            }
        }

        #endregion

        #region Public-Methods

        /// <summary>
        /// Test the terminal capabilities and display current settings
        /// </summary>
        public void TestTerminal()
        {
            Console.WriteLine("");
            Console.WriteLine("Terminal Capability Test");
            Console.WriteLine("------------------------");
            Console.WriteLine($"OS                        : {RuntimeInformation.OSDescription}");
            Console.WriteLine($"Unicode Support Detected  : {_SupportsUnicode}");
            Console.WriteLine($"Current Encoding          : {Console.OutputEncoding.EncodingName}");
            Console.WriteLine($"Current Encoding CodePage : {Console.OutputEncoding.CodePage}");
            Console.WriteLine($"WT_SESSION                : {Environment.GetEnvironmentVariable("WT_SESSION")}");
            Console.WriteLine($"TERM                      : {Environment.GetEnvironmentVariable("TERM")}");
            Console.WriteLine($"ConEmuANSI                : {Environment.GetEnvironmentVariable("ConEmuANSI")}");

            // Test if we can actually output Unicode
            Console.WriteLine("");
            Console.WriteLine("Unicode Test Output");
            Console.WriteLine("-------------------");
            Console.WriteLine("Check mark : ✓");
            Console.WriteLine("Warning    : ⚠");
            Console.WriteLine("Info       : ℹ");

            Console.WriteLine("");
            Console.WriteLine("Current Icons");
            Console.WriteLine("-------------");
            foreach (var icon in _UnicodeIcons)
            {
                Console.WriteLine($"{icon.Key,-10}: {this[icon.Key]} (Current) | ASCII: {_AsciiIcons[icon.Key]} | Unicode: {_UnicodeIcons[icon.Key]}");
            }

            Console.WriteLine("");
        }

        #endregion

        #region Private-Methods

        private bool DetectUnicodeSupport()
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    try
                    {
                        Console.Write("✓");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}