using _28._08MiniProject.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _28._08MiniProject.Models
{
    internal class Product:BaseEntity
    {

        private static int p_count = 0;
        private string _name { get; set; }
        public decimal _price { get; set; }
        public double Stock { get; set; }
        public Product(string name,decimal price,double stock)
        {
            Id = ++p_count;
            Name = name;
            Price = price;
            Stock = stock;
        }

       

        public string Name
        {
            get{return _name;}
            set
            {
                if (value.Length >= 1)
                _name = value;
            }
        }
        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value > 0)
                {
                    _price = value;
                }
            }
        }
        public void PrintInfo()
        {

        }
    }
}
