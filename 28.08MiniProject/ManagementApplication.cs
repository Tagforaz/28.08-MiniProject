using _28._08MiniProject.Models;
using _28._08MiniProject.Services;
using _28._08MiniProject.Utilities.VisualEffect;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _28._08MiniProject
{
    internal class ManagementApplication
    {
        private ProductServices productServices = new ProductServices();
        private OrderServices orderServices = new OrderServices();
        public void Run()
        {
            int num = -1;

            while (num != 0)
            {
                var menuLines = new[]
                {
                    "1. Create Product",
                    "2. Delete Product",
                    "3. Get Product By Id",
                    "4. Show All Product",
                    "5. Refill Product",
                    "6. Order Product",
                    "7. Show All Orders",
                    "8. Change Order Status",
                    "9. Cancel Order",
                    "10. Return Order",
                    "0. Quit"
                };
                Animation.PrintMenuAnimated(menuLines);
                num = ReadMenuChoice(0, 10,menuLines);

                Console.Clear();
                switch (num)
                {
                    case 1: productServices.CreateProduct(); break;
                    case 2: productServices.DeleteProduct(); break;
                    case 3: productServices.GetProductById(); break;
                    case 4: productServices.ShowAllProduct(); break;
                    case 5: productServices.RefillProduct(); break;
                    case 6: orderServices.CreateOrder(); break;
                    case 7: orderServices.ShowAllOrders(); break;
                    case 8: orderServices.ChangeOrderStatus(); break;
                    case 9: orderServices.CancelOrder(); break;
                    case 10: orderServices.ReturnOrder(); break;
                    case 0:
                        Console.WriteLine("Program ended.");
                        break;
                    default:
                        Console.Clear();
                        ConsoleTheme.WriteError("Wrong Input. Try Again.");
                        break;
                }
            }
        }
        private static int ReadMenuChoice(int min, int max, string[] menuLines)
        {
            while (true)
            {
                Console.Write("\nChoose option: ");
                var input = (Console.ReadLine() ?? "").Trim();
                if (!int.TryParse(input, out int n))
                {
                    Console.Clear();
                    Animation.PrintMenuAnimated(menuLines);
                    ConsoleTheme.WriteError("Please enter a number.");
                    continue;
                }
                if (n < min || n > max)
                {
                    Console.Clear();
                    Animation.PrintMenuAnimated(menuLines);
                    ConsoleTheme.WriteError($"Please enter a number between {min} and {max}.");
                    continue;
                }
                return n;
            }
        }
    }
}
