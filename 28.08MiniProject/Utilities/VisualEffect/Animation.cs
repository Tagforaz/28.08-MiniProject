using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _28._08MiniProject.Utilities.VisualEffect
{
    internal static class Animation
    {
        public static void PrintAsciiArt()
        {
            string[] art =
            {
                @"____________  _____  _____  ____         _____ _                 ",
                @"| ___ \ ___ \|____ ||  _  |/ ___|       /  ___| |                ",
                @"| |_/ / |_/ /    / /| |/' / /___        \ `--.| |_ ___  _ __ ___ ",
                @"|  __/| ___ \    \ \|  /| | ___ \        `--. \ __/ _ \| '__/ _ \",
                @"| |   | |_/ /.___/ /\ |_/ / \_/ |       /\__/ / || (_) | | |  __/",
                @"\_|   \____/ \____/  \___/\_____/       \____/ \__\___/|_|  \___|",
                @"                                                                 ",
                @"                                                                 "
            };

            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (var line in art)
            {
                Console.WriteLine(line);
                Thread.Sleep(100);
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void GradientText(string text, int startColor, int endColor)
        {
            int length = text.Length;
            for (int i = 0; i < length; i++)
            {
                int colorIndex = startColor + (i * (endColor - startColor)) / Math.Max(length, 1);
                int idx = Math.Clamp(colorIndex, 0, 15);
                Console.ForegroundColor = (ConsoleColor)idx;
                Console.Write(text[i]);
            }
            Console.ResetColor();
            Console.WriteLine("\n");
        }

        public static void LoadingBar()
        {
            Console.WriteLine("Loading...");
            Console.ForegroundColor = ConsoleColor.Green;

            for (int i = 0; i <= 20; i++)
            {
                Console.Write($"\r[{new string('=', i)}{new string(' ', 20 - i)}] {i * 5}%   ");
                try { Console.Beep(500 + i * 20, 50); } catch { }
                Thread.Sleep(100);
            }

            Console.ResetColor();
            Console.WriteLine("\nDownloading compleated!\n");
        }

        public static void Spinner(int durationMs)
        {
            Console.Write("Loading");
            char[] symbols = { '|', '/', '-', '\\' };
            DateTime end = DateTime.Now.AddMilliseconds(durationMs);
            int i = 0;

            while (DateTime.Now < end)
            {
                Console.Write(symbols[i++ % symbols.Length]);
                Console.SetCursorPosition(Math.Max(Console.CursorLeft - 1, 0), Console.CursorTop);
                Thread.Sleep(100);
            }
            Console.WriteLine("✔");
        }

        public static void MatrixEffect(int rows, int cycles)
        {
            Random rand = new Random();
            Console.ForegroundColor = ConsoleColor.Green;

            for (int c = 0; c < cycles; c++)
            {
                int w = Math.Max(Console.WindowWidth - 1, 1);
                int h = Math.Max(Console.WindowHeight - 1, 1);

                for (int i = 0; i < rows; i++)
                {
                    int x = rand.Next(w);
                    int y = rand.Next(h);
                    try
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(rand.Next(0, 2));
                    }
                    catch { }
                }
                Thread.Sleep(100);
            }

            Console.ResetColor();
            Console.Clear();
        }
    }
}
