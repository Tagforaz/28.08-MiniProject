using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _28._08MiniProject.Utilities.VisualEffect
{
    internal static class Animation
    {
        public static void Run()
        {
            Console.Clear();
            Console.CursorVisible = false;
            var prevColor = Console.ForegroundColor;


            MatrixRain(durationMs: 3000);


            Console.Clear();
            BigTitlePB306();

         
            TypeLine("loading your datas...", ConsoleColor.Green, 14);
            ProgressBar(steps: 34, totalMs: 1200);

            Console.ForegroundColor = prevColor;
            Console.CursorVisible = true;
            Console.Clear();
        }

      
        private static void BigTitlePB306()
        {
            
            string[] lines =
{
    "PPPP   BBBB    3333    000   6666         SSSS   TTTTTT   OOOO   RRRR   EEEEE ",
    "P   P  B   B  3    3  0   0 6            S    S    TT    O    O  R   R  E     ",
    "P   P  B   B       3  0   0 6            S         TT    O    O  R   R  E     ",
    "PPPP   BBBB     333   0   0 6666          SSS      TT    O    O  RRRR   EEEE  ",
    "P      B   B       3  0   0 6   6             S    TT    O    O  R R    E     ",
    "P      B   B  3    3  0   0 6   6        S    S    TT    O    O  R  R   E     ",
    "P      BBBB    3333    000   666         SSSS     TT      OOOO   R   R  EEEEE ",
};

            int w = SafeWidth();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(); 
            foreach (var line in lines)
                WriteCentered(line, w);

            Console.WriteLine();
        }


        private static void MatrixRain(int durationMs)
        {
            var rnd = new Random();
            int w = SafeWidth();
            int h = SafeHeight();

         
            int[] drops = new int[w];
            for (int i = 0; i < w; i++)
                drops[i] = rnd.Next(h);

            var end = DateTime.Now.AddMilliseconds(durationMs);

            Console.ForegroundColor = ConsoleColor.Green;
            while (DateTime.Now < end)
            {
                for (int i = 0; i < w; i++)
                {
                    
                    int y = drops[i];
                    try
                    {
                        Console.SetCursorPosition(i, y);
                        Console.Write(rnd.Next(2) == 0 ? '0' : '1');
                    }
                    catch {  }

                   
                    drops[i] = (y + 1) % h;
                }
                Thread.Sleep(50); 
            }
            Console.ResetColor();
        }

        private static void TypeLine(string text, ConsoleColor color, int delayMs)
        {
            Console.ForegroundColor = color;
            foreach (var ch in text)
            {
                Console.Write(ch);
                Thread.Sleep(delayMs);
            }
            Console.ResetColor();
            Console.WriteLine("\n");
        }

        private static void ProgressBar(int steps, int totalMs)
        {
            int barWidth = Math.Clamp(SafeWidth() - 12, 10, 50);
            int delay = Math.Max(totalMs / steps, 1);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Loading ");
            

            for (int i = 0; i <= steps; i++)
            {
                int filled = (int)Math.Round((double)i / steps * barWidth);
                string bar = "[" + new string('=', filled) + new string(' ', barWidth - filled) + $"] {(i * 100 / steps),3}%";
                Console.Write("\r" + bar);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }


        private static void WriteCentered(string text, int windowWidth)
        {
            int left = Math.Max((windowWidth - text.Length) / 2, 0);
            try { Console.SetCursorPosition(left, Console.CursorTop); } catch { }
            Console.WriteLine(text);
        }

        private static int SafeWidth()
        {
            try { return Math.Max(Console.WindowWidth, 40); }
            catch { return 80; }
        }

        private static int SafeHeight()
        {
            try { return Math.Max(Console.WindowHeight, 20); }
            catch { return 25; }
        }


        public static void PrintMenuAnimated(string[] lines, int charDelayMs = 15, int lineDelayMs = 60)
        {
            Console.Clear();
            ConsoleTheme.SetMainMenu();

            foreach (var line in lines)
            {
                foreach (var ch in line)
                {
                    Console.Write(ch);
                    System.Threading.Thread.Sleep(charDelayMs);
                }
                Console.WriteLine();
                System.Threading.Thread.Sleep(lineDelayMs);
            }

            ConsoleTheme.Reset(); 
        }
    }
}

