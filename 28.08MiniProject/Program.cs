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

            Animation.Run();
            var app = new ManagementApplication();
            app.Run();
        }
    }
}
