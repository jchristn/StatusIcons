namespace Test.Automated
{
    using System.Threading.Tasks;
    using Test.Shared;
    using Touchstone.Cli;

    /// <summary>
    /// Automated Touchstone console runner over the shared StatusIcon suites.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main entry point.
        /// </summary>
        /// <param name="args">Command line arguments. Supports "--results &lt;path&gt;" for JSON export.</param>
        /// <returns>Exit code (0 for success, 1 for failures).</returns>
        public static async Task<int> Main(string[] args)
        {
            string resultsPath = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--results" && i + 1 < args.Length)
                {
                    resultsPath = args[i + 1];
                    break;
                }
            }

            return await ConsoleRunner.RunAsync(StatusIconSuites.All, resultsPath: resultsPath).ConfigureAwait(false);
        }
    }
}
