using _28._08MiniProject.Utilities.VisualEffect;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace _28._08MiniProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.OutputEncoding = new System.Text.UTF8Encoding(false);
            //Console.TreatControlCAsInput = true;
            //Animation.PrintAsciiArt();
            //Animation.GradientText("Welcome to PB306 STORE", 1, 15);
            //Animation.LoadingBar();
            //Animation.Spinner(3000);
            //Animation.MatrixEffect(20, 90);
            //Console.ResetColor();

            var app = new ManagementApplication();
            app.Run();
        }
    }
}
