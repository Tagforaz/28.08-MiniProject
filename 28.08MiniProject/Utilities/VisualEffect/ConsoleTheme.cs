using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _28._08MiniProject.Utilities.VisualEffect
{
    internal static class ConsoleTheme
    {
        private static ConsoleColor _backup = Console.ForegroundColor;

        public static void SetMainMenu() { _backup = Console.ForegroundColor; Console.ForegroundColor = ConsoleColor.Green; }
        public static void SetSubmenu() { _backup = Console.ForegroundColor; Console.ForegroundColor = ConsoleColor.Yellow; }
        public static void Reset() { Console.ForegroundColor = _backup; }

        public static void WriteError(string message)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = prev;
        }

      
        public static void WriteInfo(string message)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = prev;
        }
    }
}
