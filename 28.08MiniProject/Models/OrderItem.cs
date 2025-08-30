using _28._08MiniProject.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace _28._08MiniProject.Models
{
    internal class OrderItem:BaseEntity
    {
        private static int oi_count;
        public List<Product>? Products { get; set; }
        public double Count { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal SubTotal {  get; set; }
        public OrderItem()
        {
            Id = ++oi_count;
        }

    }
}
