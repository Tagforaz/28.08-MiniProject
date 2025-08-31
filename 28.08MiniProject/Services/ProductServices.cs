using _28._08MiniProject.Models;
using _28._08MiniProject.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _28._08MiniProject.Services
{
    internal class ProductServices
    {
        public static string path = @"C:\Users\Tagforaz\Desktop\Pb306\28.08MiniProject\28.08MiniProject\Data\Product.json";
        public Repository ProductRepository { get; set; } = new Repository();
        public void CreateProduct()
        {
            Console.WriteLine("Please input product's info:");
            string? name;
            do
            {
                Console.WriteLine("Please input product's name(minimum one character):");
                name = Console.ReadLine().ToUpper().Trim();
                if (string.IsNullOrEmpty(name) || name.Length < 1)
                {
                    Console.Clear();
                    Console.WriteLine("Name must be at least one character long.");
                }
            } while (string.IsNullOrEmpty(name) || name.Length < 1); 
            decimal price;
            do
            {
                Console.WriteLine("Please input product's price (must be greater than 0):");
                string priceInput = Console.ReadLine().Trim();
                if (!decimal.TryParse(priceInput, out price) || price <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("Price will not null and must be a valid number greater than 0.");
                }
            } while (price <= 0);
            double stock;
            do
            {
                Console.WriteLine("Please input product's stock (must be greater than 0):");
                string stockInput = Console.ReadLine().Trim();
                if (!double.TryParse(stockInput, out stock) || stock <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("Stock will not null and must be a valid number greater than 0.");
                }
            } while (stock <= 0);
            Console.WriteLine("Product Created");
            Product product = new Product(name, price, stock);
            List<Product> products = ReadProduct();
            products.Add(product);
            WriteProduct(products);
            

        }
        public void DeleteProduct()
        {
            ShowAllProduct();
            Console.WriteLine("Please,input product's id for delete:");
            Guid id;
            while (!Guid.TryParse(Console.ReadLine(), out id))
            {
                Console.Clear();
                Console.WriteLine("Wrong Id. Please enter a product ID.");
            }
            List<Product> products = ReadProduct();
            var idToDelete = products.FirstOrDefault(p => p.Id == id);
            if (idToDelete != null)
            {
                products.Remove(idToDelete);
                WriteProduct(products); 
                Console.WriteLine("Product successfully deleted.");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Product not found.");
            }
        }
        public void GetProductById()
        {
            Console.WriteLine("Please input product's id for product's info:");

            Guid id;
            while (!Guid.TryParse(Console.ReadLine(), out id))
            {
                Console.Clear();
                Console.WriteLine("Wrong Id. Please enter a product ID.");
            }
            List<Product> products = ReadProduct();
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                product.PrintInfo();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Product not found.");
            }
        }
        public void ShowAllProduct()
        {
            List<Product> products = ReadProduct();
            if (products.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("You have noy any product.");
                return;
            }
            Console.WriteLine("All Product's Info:");
            foreach (var product in products)
            {
                string stockStatus;
                if (product.Stock > 0)
                {
                    stockStatus = $"Stock: {product.Stock}";
                }
                else
                {
                    stockStatus = "Out of Stock";
                }
                Console.WriteLine($"Id: {product.Id}, Product: {product.Name}, Price: {product.Price:C}, {stockStatus}");
            }
        }
        public void RefillProduct()
        {
            ShowAllProduct();
            Console.WriteLine("Please input product's id to refill stock:(negative number=>decrease,positive number=>increase)");
            Guid id;
            while (!Guid.TryParse(Console.ReadLine(), out id))
            {
                Console.Clear();
                Console.WriteLine("Wrong Id. Please enter a valid product ID.");
            }
            List<Product> products = ReadProduct();
            var productToRefill = products.FirstOrDefault(p => p.Id == id);
            if (productToRefill != null)
            {
                double refillAmount;
                do
                {
                    Console.WriteLine("Please input the amount to refill:");
                    string input = Console.ReadLine();
                    if (!double.TryParse(input, out refillAmount))
                    {
                        Console.WriteLine("Please enter a valid number.");
                        continue;
                    }
                    if (productToRefill.Stock + refillAmount < 0)
                    {
                        Console.Clear();
                        Console.WriteLine("Stock must be greater than 0.");
                    }
                    else
                    {
                        productToRefill.Stock += refillAmount;
                        WriteProduct(products);
                        Console.WriteLine($"{productToRefill.Name} stock refilled. New stock: {productToRefill.Stock}");
                    }
                } while (productToRefill.Stock + refillAmount < 0); 
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Product not found.");
            }
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