using _28._08MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _28._08MiniProject.Services
{
    internal class ProductServices
    {
        static List<string> products = new List<string> ();
        string path = @"C:\Users\Tagforaz\Desktop\Pb306\28.08MiniProject\28.08MiniProject\Data\product.json";
        public void CreateProduct()
        {
            Console.WriteLine("Please input product's name,price and stock");
            
            
            int num2;
            string answer = Console.ReadLine();
            int.TryParse(answer, out num2);
            Product product = default;
          
            
        }
    }
}
