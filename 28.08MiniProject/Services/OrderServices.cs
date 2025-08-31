using _28._08MiniProject.Models;
using _28._08MiniProject.Repositories;
using _28._08MiniProject.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _28._08MiniProject.Services
{

    internal class OrderServices
    {
        public static string path = @"C:\Users\Tagforaz\Desktop\Pb306\28.08MiniProject\28.08MiniProject\Data\Orders.json";
        public Repository OrderRepository { get; set; } = new Repository();
        public void CreateOrder()
        {
            Console.WriteLine("Enter buyer email (must contain '@'):");
            string email;
            while (true)
            {
                email = (Console.ReadLine() ?? "").Trim();
                if (!string.IsNullOrEmpty(email) && email.Contains("@")) break;
                Console.Clear();
                Console.WriteLine("Wrong email.Input again:");
            }
            var products = ProductServices.ReadProduct();
            if (products.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("There are no products to order.");
                return;
            }
            var orderItems = new List<OrderItem>();
            bool addMore;
            do
            {
                Console.WriteLine("Enter product Id:");
                Guid productId;
                while (!Guid.TryParse(Console.ReadLine(), out productId))
                {
                    Console.Clear();
                    Console.WriteLine("Wrong Id.Input again:");
                }
                var product = products.FirstOrDefault(p => p.Id == productId);
                if (product == null)
                {
                    Console.Clear();
                    Console.WriteLine("Product not found.");
                    addMore = AskYesNo("Add another product? (y/n): ");
                    continue;
                }
                Console.WriteLine($"How many '{product.Name}'? In stock: {product.Stock}");
                double count;
                while (!double.TryParse(Console.ReadLine(), out count) || count <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("Count must be positive. Try again:");
                }
                if (count > product.Stock)
                {
                    Console.Clear();
                    Console.WriteLine("Not enough stock.");
                }
                else
                {
                    product.Stock -= count;
                    var oi = new OrderItem
                    {
                        Products = new List<Product> { product },
                        Count = count,
                        ProductPrice = product.Price,
                        SubTotal = product.Price * (decimal)count
                    };
                    orderItems.Add(oi);
                    Console.WriteLine($"Added: {product.Name} x {count}");
                }
                addMore = AskYesNo("Add another product? (y/n): ");
            } while (addMore);
            if (orderItems.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("No items selected. Order cancelled.");
                return;
            }
            ProductServices.WriteProduct(products);
            var order = new Order
            {
                Items = orderItems,
                Email = email,
                Status = OrderStatus.Pending,
                OrderedAt = DateTime.Now,
                Total = (int)orderItems.Sum(i => i.SubTotal)
            };
            var orders = OrderRepository.Deserialize<Order>(path);
            orders.Add(order);
            OrderRepository.Serialize(orders, path);
            Console.WriteLine("\nOrder created successfully!");
            Console.WriteLine($"Order Id: {order.Id}");
            Console.WriteLine($"Email: {order.Email}");
            Console.WriteLine($"Status: {order.Status}");
            Console.WriteLine($"Total: {order.Total}");
        }
        private static bool AskYesNo(string question)
        {
            Console.Write(question);
            var s = (Console.ReadLine() ?? "").Trim().ToLower();
            return s == "y" || s == "yes";
        }
        public void ShowAllOrders()
        {
            var orders = OrderRepository.Deserialize<Order>(path);
            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found.");
                return;
            }
            Console.WriteLine("All Orders:\n");
            foreach (var order in orders)
            {
                Console.WriteLine($"Order Id: {order.Id}");
                Console.WriteLine($"Email: {order.Email}");
                Console.WriteLine($"Status: {order.Status}");
                Console.WriteLine($"Ordered At: {order.OrderedAt}");

                Console.WriteLine("Items:");
                foreach (var item in order.Items)
                {
                    Console.WriteLine($"   - {item.Products.First().Name} x {item.Count} @ {item.ProductPrice:C} = {item.SubTotal:C}");
                }
                Console.WriteLine($"TOTAL: {order.Total}");
                Console.WriteLine(new string('-', 45));
            }
        }
        public void ChangeOrderStatus()
        {
            var orders = OrderRepository.Deserialize<Order>(path);
            if (orders.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("No orders found.");
                return;
            }
            Console.WriteLine("Enter Order Id:");
            Guid id;
            while (!Guid.TryParse(Console.ReadLine(), out id))
            {
                Console.Clear();
                Console.WriteLine("Wrong Id.Input again:");
            }
            var order = orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                Console.Clear();
                Console.WriteLine("Order not found.");
                return;
            }
            Console.WriteLine("Choose new status:");
            Console.WriteLine("1) Pending");
            Console.WriteLine("2) Confirmed");
            Console.WriteLine("3) Completed");
            int opt;
            while (!int.TryParse(Console.ReadLine(), out opt) || opt < 1 || opt > 3)
            {
                Console.Clear();
                Console.WriteLine("Wrong option. Enter 1, 2 or 3:");
            }
            order.Status = (Utilities.Enums.OrderStatus)opt;
            OrderRepository.Serialize(orders, path);
            Console.WriteLine($"Order status updated to {order.Status}.");
        }
    }
}