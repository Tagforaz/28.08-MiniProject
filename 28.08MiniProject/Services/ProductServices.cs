using _28._08MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using _28._08MiniProject.Repositories;

namespace _28._08MiniProject.Services
{
    internal class ProductServices
    {
        public static string path = @"C:\Users\Tagforaz\Desktop\Pb306\28.08MiniProject\28.08MiniProject\Data\Product.json";
        public Repository ProductRepository { get; set; } = new Repository();
        public void CreateProduct()
        {
            Console.WriteLine("Please input product's name, price, and stock:");
            string? name = Console.ReadLine();
            decimal price = Convert.ToDecimal(Console.ReadLine());
            double stock = Convert.ToDouble(Console.ReadLine());

            Product product = new Product(name, price, stock);
            List<Product> products = ReadProduct();
            products.Add(product);
            WriteProduct(products);

        }
        public static List<Product> ReadProduct()
        {
            return new Repository().Deserialize<Product>(path);
        }
        public static void WriteProduct(List<Product> products)
        {
            new Repository().Serialize(products, path);
        }
    }
   }