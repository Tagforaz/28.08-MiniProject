using _28._08MiniProject.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace _28._08MiniProject.Models
{
    internal class Product:BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double Stock { get; set; }
        public Product(string name,decimal price,double stock)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            Stock = stock;
        }
        public void PrintInfo()
        {
            Console.WriteLine($"Id : {Id}, Product Name: {Name}, Price: {Price}, Stock: {Stock}");
        }
    }
}
