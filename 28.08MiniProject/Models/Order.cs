using _28._08MiniProject.Models.Base;
using _28._08MiniProject.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace _28._08MiniProject.Models
{
    internal class Order:BaseEntity
    {
        private static int o_count = 0;
        public List<OrderItem>? Items {  get; set; }
        public int Total { get; set; }
        public string? Email {  get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderedAt { get; set; }
        public Order()
        {
            Id = ++o_count;
        }
    }
}
