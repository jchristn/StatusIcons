namespace Test
{
    using StatusIcons;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Testing terminal");
            Console.WriteLine("----------------");
            StatusIcon icon = new StatusIcon();
            icon.TestTerminal();

            Console.WriteLine("Enumerating icons");
            Console.WriteLine("-----------------");
            ConcurrentDictionary<string, string> dict = icon.Icons;
            foreach (KeyValuePair<string, string> item in dict)
            {
                Console.WriteLine(item.Key + ": " + item.Value);
            }

            Console.WriteLine("");
            Console.WriteLine("Testing custom icons");
            Console.WriteLine("--------------------");
            
            icon["Speaker"] = "🔊";
            Console.WriteLine("Icon 'Speaker' : " + icon["Speaker"]);

            icon.UnicodeIcons["Music"] = "♫";
            icon.AsciiIcons["Music"] = ":(";
            Console.WriteLine("Icon 'Music'   : " + icon["Music"]);
            Console.WriteLine("");
        }
    }
}