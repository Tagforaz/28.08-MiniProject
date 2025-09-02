using _28._08MiniProject.Models;
using _28._08MiniProject.Repositories;
using _28._08MiniProject.Utilities.Enums;
namespace _28._08MiniProject.Services
{
    internal class OrderServices
    {
        public static string path = @"C:\Users\Tagforaz\Desktop\Pb306\28.08MiniProject\28.08MiniProject\Data\Orders.json";
        public Repository OrderRepository { get; set; } = new Repository();
        public void CreateOrder()
        {
            Console.WriteLine("ORDER MENU");
            var products = ProductServices.ReadProduct();
            if (products.Count == 0)
            {
                Console.WriteLine("There are no products to order.");
                return;
            }
            string email = "";
            var orderItems = new List<OrderItem>();
            int step = 0;
            Product? chosen = null;
            Guid productId = Guid.Empty;
            double count = 0;
            bool ordering = true;
            while (ordering)
            {
                if (step == 0)
                {
                    Console.WriteLine("Write 'menu' = back to menu \nEnter user email: (must contain '@')  ");
                    var input = Console.ReadLine()?.Trim() ?? "";
                    if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;
                    if (!string.IsNullOrWhiteSpace(input) && input.Contains("@"))
                    {
                        email = input;
                        step = 1;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Wrong email. Try again.");
                    }
                }
                else if (step == 1)
                {
                    Console.WriteLine(new string('-', 80));
                    new ProductServices().ShowAllProduct();
                    Console.WriteLine(new string('-', 80));
                    Console.WriteLine("Write 'back' = back to email, 'menu' = back to menu\nEnter product Id:  ");
                    var input = Console.ReadLine()?.Trim() ?? "";
                    if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;
                    if (input.Equals("back", StringComparison.OrdinalIgnoreCase)) { step = 0; continue; }
                    if (!Guid.TryParse(input, out productId))
                    {
                        Console.Clear();
                        Console.WriteLine("Wrong Id. Try again.");
                        continue;
                    }
                    chosen = products.FirstOrDefault(p => p.Id == productId);
                    Console.Clear();
                    if (chosen == null) { Console.WriteLine("Product not found."); continue; }
                    if (chosen.Stock <= 0) { Console.WriteLine("Selected product is out of stock."); continue; }
                    step = 2;
                }
                else
                {
                    Console.WriteLine($"Write 'back' = back to product, 'menu' = back to menu\nHow many '{chosen!.Name}'? In stock: {chosen.Stock} ");
                    var input = Console.ReadLine()?.Trim() ?? "";
                    if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;
                    if (input.Equals("back", StringComparison.OrdinalIgnoreCase)) { step = 1; continue; }
                    if (!double.TryParse(input, out count) || count <= 0)
                    {
                        Console.Clear();
                        Console.WriteLine("Count must be positive.");
                        continue;
                    }
                    if (count > chosen.Stock)
                    {
                        Console.Clear();
                        Console.WriteLine("Not enough stock.");
                        continue;
                    }
                    chosen.Stock -= count;
                    orderItems.Add(new OrderItem
                    {
                        Products = new List<Product> { chosen },
                        Count = count,
                        ProductPrice = chosen.Price,
                        SubTotal = chosen.Price * (decimal)count
                    });
                    Console.WriteLine($"Added: {chosen.Name} x {count}");
                    bool addMore = AskYesNo("Add another product? (y/n): ");
                    if (addMore)
                    {
                        step = 1;
                        chosen = null;
                        productId = Guid.Empty;
                        count = 0;
                    }
                    else
                    {
                        ordering = false;
                    }
                }
            }
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
            Console.WriteLine($"Order Id: {order.Id}\nEmail: {order.Email}\nStatus: {order.Status}\nTotal: {order.Total}");
        }
        private static bool AskYesNo(string question)
        {
            Console.Write(question);
            var s = (Console.ReadLine() ?? "").Trim().ToLower();
            return s == "y" || s == "yes";
        }
        public void ShowAllOrders()
        {
            Console.WriteLine("ORDER MENU");
            var orders = OrderRepository.Deserialize<Order>(path);
            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found.");
                return;
            }
            Console.WriteLine("All Orders:\n");
            foreach (var order in orders)
            {
                Console.WriteLine($"Order Id: {order.Id} \nEmail: {order.Email} \nStatus: {order.Status} \nOrdered At: {order.OrderedAt} ");
                Console.WriteLine("Items:");
                foreach (var item in order.Items)
                {
                    Console.WriteLine($" - {item.Products.First().Name} x {item.Count} @ {item.ProductPrice:C} = {item.SubTotal:C}");
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
            int step = 0;
            Guid orderId = Guid.Empty;
            Order? order = null;
            var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
            string? error = null;
            string? statusError = null;
            while (true)
            {
                if (step == 0)
                {
                    Console.Clear();
                    Console.WriteLine(new string('-', 80));
                    ShowAllOrders();
                    Console.WriteLine(new string('-', 80));
                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine(error);
                        error = null;
                    }
                    Console.Write("Write 'menu' for back to menu.\nInput Order Id: ");
                    var input = Console.ReadLine()?.Trim() ?? "";
                    if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        error = "Id can't be empty.Input again.";
                        continue;
                    }
                    if (!Guid.TryParse(input, out orderId))
                    {
                        error = "Wrong Id.Input again.";
                        continue;
                    }
                    order = orders.FirstOrDefault(o => o.Id == orderId);
                    if (order == null)
                    {
                        error = "Order not found.Input again.";
                        continue;
                    }
                    step = 1;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"Order: {order!.Id}");
                    Console.WriteLine($"Current Status: {order.Status}");
                    Console.WriteLine("Write 'back' for back, 'menu' for back to menu\nChoose new status:");
                    for (int i = 0; i < statuses.Count; i++)
                        Console.WriteLine($"{i + 1}) {statuses[i]}");
                    if (!string.IsNullOrEmpty(statusError))
                    {
                        Console.WriteLine(statusError);
                        statusError = null;
                    }
                    var input = Console.ReadLine()?.Trim() ?? "";
                    if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;
                    if (input.Equals("back", StringComparison.OrdinalIgnoreCase)) { step = 0; continue; }

                    if (!int.TryParse(input, out int opt) || opt < 1 || opt > statuses.Count)
                    {
                        statusError = "Wrong option.Please select a number from the list.";
                        continue;
                    }
                    var newStatus = statuses[opt - 1];
                    if (newStatus == order.Status)
                    {
                        statusError = $"Order is already in '{order.Status}' status. Choose another.";
                        continue;
                    }
                    order.Status = newStatus;
                    OrderRepository.Serialize(orders, path);
                    Console.WriteLine($"Order status updated to {order.Status}.");
                    break;
                }
            }
        }
    }
}