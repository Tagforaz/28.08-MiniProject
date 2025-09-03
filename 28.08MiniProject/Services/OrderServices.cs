using _28._08MiniProject.Models;
using _28._08MiniProject.Repositories;
using _28._08MiniProject.Utilities.Enums;
using _28._08MiniProject.Utilities.VisualEffect;
namespace _28._08MiniProject.Services
{
    internal class OrderServices
    {
        private readonly static string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Data", "Orders.json");
        public Repository OrderRepository { get; set; } = new Repository();
        public void CreateOrder()
        {
            ConsoleTheme.SetSubmenu();
            try
            {
                Console.WriteLine("ORDER MENU");
                var products = ProductServices.ReadProduct();
                if (products.Count == 0)
                {
                    ConsoleTheme.WriteError("There are no products to order.");
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
                        Console.WriteLine("Write 'menu' = back to menu");
                        Console.Write("Enter user email (must contain '@'): ");
                        var input = (Console.ReadLine() ?? "").Trim();
                        if (input.Equals("menu", StringComparison.OrdinalIgnoreCase))
                            return;
                        if (!string.IsNullOrWhiteSpace(input) && input.Contains("@"))
                        {
                            email = input;
                            step = 1;
                        }
                        else
                        {
                            Console.Clear();
                            ConsoleTheme.WriteError("Wrong email. Try again.");
                        }
                    }
                    else if (step == 1)
                    {
                        Console.WriteLine(new string('-', 80));
                        new ProductServices().ShowAllProduct(false);
                        Console.WriteLine(new string('-', 80));
                        Console.WriteLine("Write 'back' = back to email, 'menu' = back to menu");
                        Console.Write("Enter product Id: ");
                        var input = (Console.ReadLine() ?? "").Trim();
                        if (input.Equals("menu", StringComparison.OrdinalIgnoreCase))
                            return;

                        if (input.Equals("back", StringComparison.OrdinalIgnoreCase))
                        {
                            step = 0;
                            continue;
                        }

                        if (!Guid.TryParse(input, out productId))
                        {
                            Console.Clear();
                            ConsoleTheme.WriteError("Wrong Id. Try again.");
                            continue;
                        }
                        chosen = products.FirstOrDefault(p => p.Id == productId);
                        if (chosen == null)
                        {
                            Console.Clear();
                            ConsoleTheme.WriteError("Product not found.");
                            continue;
                        }
                        if (chosen.Stock <= 0)
                        {
                            Console.Clear();
                            ConsoleTheme.WriteError("Selected product is out of stock.");
                            continue;
                        }
                        step = 2;
                    }
                    else
                    {
                        Console.WriteLine($"Write 'back' = back to product, 'menu' = back to menu");
                        Console.Write($"How many '{chosen!.Name}'? In stock: {chosen.Stock}  => ");
                        var input = (Console.ReadLine() ?? "").Trim();
                        if (input.Equals("menu", StringComparison.OrdinalIgnoreCase))
                            return;
                        if (input.Equals("back", StringComparison.OrdinalIgnoreCase))
                        {
                            step = 1;
                            continue;
                        }

                        if (!double.TryParse(input, out count) || count <= 0)
                        {
                            Console.Clear();
                            ConsoleTheme.WriteError("Count must be positive.");
                            continue;
                        }
                        if (count > chosen.Stock)
                        {
                            Console.Clear();
                            ConsoleTheme.WriteError("Not enough stock.");
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
                    ConsoleTheme.WriteError("No items selected. Order cancelled.");
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
                Console.ReadKey();
            }
            finally
            {
                ConsoleTheme.Reset();
            }
        }
        private static bool AskYesNo(string question)
        {
            Console.Write(question);
            var s = (Console.ReadLine() ?? "").Trim().ToLower();
            return s == "y" || s == "yes";
        }
        public void ShowAllOrders(bool pause = true)
        {
            ConsoleTheme.SetSubmenu();
            try
            {
                Console.WriteLine("ORDER MENU");
                var orders = OrderRepository.Deserialize<Order>(path);
                if (orders.Count == 0)
                {
                    ConsoleTheme.WriteError("No orders found.");
                    return;
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Order Id                             | Email              | Status     | Ordered At         | Total");
                Console.WriteLine(new string('-', 100));
                Console.ResetColor();
                foreach (var order in orders)
                {
                    Console.Write($"{order.Id} | {order.Email,-18} | ");
                    switch (order.Status)
                    {
                        case OrderStatus.Pending:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case OrderStatus.Confirmed:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case OrderStatus.Completed:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case OrderStatus.Cancelled:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case OrderStatus.Returned:
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            break;
                        default:
                            Console.ResetColor();
                            break;
                    }
                    Console.Write($"{order.Status,-10}");
                    Console.ResetColor();
                    Console.WriteLine($" | {order.OrderedAt:yyyy-MM-dd HH:mm} | {order.Total,8:C}");
                    foreach (var item in order.Items)
                    {
                        var productName = item.Products.FirstOrDefault()?.Name ?? "Unknown";
                        Console.WriteLine($"   -> {productName,-20} x {item.Count,-5} @ {item.ProductPrice,8:C} = {item.SubTotal,8:C}");
                    }
                    Console.WriteLine(new string('-', 100));
                }
                if (pause)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
            finally
            {
                ConsoleTheme.Reset();
            }
        }
        public void ChangeOrderStatus()
        {
            ConsoleTheme.SetSubmenu();
            try
            {
                var orders = OrderRepository.Deserialize<Order>(path);
                if (orders.Count == 0)
                {
                    ConsoleTheme.WriteError("No orders found.");
                    return;
                }
                Console.WriteLine(new string('-', 80));
                ShowAllOrders(false);
                Console.WriteLine(new string('-', 80));
                Guid orderId = Guid.Empty;
                Order? order = null;
                var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
                int step = 0; 
                while (true)
                {
                    if (step == 0)
                    {
                        Console.Write("Write 'menu' to go menu, 'list' to show orders again.\nInput Order Id: ");
                        var input = (Console.ReadLine() ?? "").Trim();
                        if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;
                        if (input.Equals("list", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.Clear();
                            Console.WriteLine(new string('-', 80));
                            ShowAllOrders(false);
                            Console.WriteLine(new string('-', 80));
                            continue;
                        }
                        if (string.IsNullOrWhiteSpace(input))
                        {
                            Console.Clear();
                            ConsoleTheme.WriteError("Id can't be empty. Input again.");
                            continue;
                        }
                        if (!Guid.TryParse(input, out orderId))
                        {
                            Console.Clear();
                            ConsoleTheme.WriteError("Wrong Id. Input again.");
                            continue;
                        }
                        order = orders.FirstOrDefault(o => o.Id == orderId);
                        if (order == null)
                        {
                            Console.Clear();
                            ConsoleTheme.WriteError("Order not found. Input again.");
                            continue;
                        }
                        step = 1;
                    }
                    else 
                    {
                        Console.WriteLine($"\nOrder: {order!.Id}");
                        Console.WriteLine($"Current Status: {order.Status}");
                        Console.WriteLine("Write 'back' to choose another order, 'menu' to go menu, 'list' to show orders again.");
                        Console.WriteLine("Choose new status:");
                        for (int i = 0; i < statuses.Count; i++)
                            Console.WriteLine($"{i + 1}) {statuses[i]}");
                        var input = (Console.ReadLine() ?? "").Trim();
                        if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;
                        if (input.Equals("list", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.Clear();
                            Console.WriteLine(new string('-', 80));
                            ShowAllOrders(false);
                            Console.WriteLine(new string('-', 80));
                            continue;
                        }
                        if (input.Equals("back", StringComparison.OrdinalIgnoreCase))
                        {
                            step = 0;
                            continue;
                        }
                        if (!int.TryParse(input, out int opt) || opt < 1 || opt > statuses.Count)
                        {
                            Console.Clear();
                            ConsoleTheme.WriteError("Wrong option. Please select a number from the list.");
                            continue;
                        }
                        var newStatus = statuses[opt - 1];
                        if (newStatus == order.Status)
                        {
                            Console.Clear();
                            ConsoleTheme.WriteError($"Order is already in '{order.Status}' status. Choose another.");
                            continue;
                        }
                        order.Status = newStatus;
                        OrderRepository.Serialize(orders, path);
                        Console.WriteLine($"Order status updated to {order.Status}.");
                        Console.ReadKey();
                        break;
                    }
                }
            }
            finally
            {
                ConsoleTheme.Reset();
            }
        }
        public void CancelOrder()
        {
            ConsoleTheme.SetSubmenu();
            try
            {
                var orders = OrderRepository.Deserialize<Order>(path);
                if (orders.Count == 0)
                {
                    ConsoleTheme.WriteError("No orders found.");
                    return;
                }
                Console.WriteLine(new string('-', 80));
                ShowAllOrders(false);
                Console.WriteLine(new string('-', 80));
                while (true)
                {
                    Console.Write("Write 'menu' to go menu, 'list' to show orders again.\nInput Order Id for cancel: ");
                    var s = (Console.ReadLine() ?? "").Trim();

                    if (s.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;

                    if (s.Equals("list", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.Clear();
                        Console.WriteLine(new string('-', 80));
                        ShowAllOrders(false);
                        Console.WriteLine(new string('-', 80));
                        continue;
                    }
                    if (!Guid.TryParse(s, out Guid id))
                    {
                        Console.Clear();
                        ConsoleTheme.WriteError("Wrong Id. Please input again.");
                        continue;
                    }
                    var order = orders.FirstOrDefault(o => o.Id == id);
                    if (order == null)
                    {
                        Console.Clear();
                        ConsoleTheme.WriteError("Order not found. Try again.");
                        continue;
                    }
                    if (order.Status != OrderStatus.Pending && order.Status != OrderStatus.Confirmed)
                    {
                        Console.Clear();
                        ConsoleTheme.WriteError("Only Pending and Confirmed orders can be cancelled.");
                        continue;
                    }
                    var products = ProductServices.ReadProduct();
                    foreach (var item in order.Items ?? new List<OrderItem>())
                    {
                        var prod = item.Products?.FirstOrDefault();
                        if (prod == null) continue;
                        var original = products.FirstOrDefault(p => p.Id == prod.Id);
                        if (original != null) original.Stock += item.Count;
                    }
                    ProductServices.WriteProduct(products);
                    order.Status = OrderStatus.Cancelled;
                    OrderRepository.Serialize(orders, path);
                    Console.WriteLine("Order cancelled and stock refilled.");
                    Console.ReadKey();
                    break;
                }
            }
            finally
            {
                ConsoleTheme.Reset();
            }
        }
        public void ReturnOrder()
        {
            ConsoleTheme.SetSubmenu();
            try
            {
                var orders = OrderRepository.Deserialize<Order>(path);
                if (orders.Count == 0)
                {
                    ConsoleTheme.WriteError("No orders found.");
                    return;
                }
                Console.WriteLine(new string('-', 80));
                ShowAllOrders(false);
                Console.WriteLine(new string('-', 80));
                while (true)
                {
                    Console.Write("Write 'menu' to go menu, 'list' to show orders again.\nInput Order Id: ");
                    var s = (Console.ReadLine() ?? "").Trim();

                    if (s.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;

                    if (s.Equals("list", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.Clear();
                        Console.WriteLine(new string('-', 80));
                        ShowAllOrders(false);
                        Console.WriteLine(new string('-', 80));
                        continue;
                    }
                    if (!Guid.TryParse(s, out Guid id))
                    {
                        Console.Clear();
                        ConsoleTheme.WriteError("Wrong Id. Please input again.");
                        continue;
                    }
                    var order = orders.FirstOrDefault(o => o.Id == id);
                    if (order == null)
                    {
                        Console.Clear();
                        ConsoleTheme.WriteError("Order not found. Try again.");
                        continue;
                    }
                    if (order.Status != OrderStatus.Completed)
                    {
                        Console.Clear();
                        ConsoleTheme.WriteError("Only Completed orders can be returned.");
                        continue;
                    }
                    var products = ProductServices.ReadProduct();
                    foreach (var item in order.Items ?? new List<OrderItem>())
                    {
                        var prod = item.Products?.FirstOrDefault();
                        if (prod == null) continue;
                        var original = products.FirstOrDefault(p => p.Id == prod.Id);
                        if (original != null) original.Stock += item.Count;
                    }
                    ProductServices.WriteProduct(products);
                    order.Status = OrderStatus.Returned;
                    OrderRepository.Serialize(orders, path);
                    Console.WriteLine("Order returned and stock refilled.");
                    Console.ReadKey();
                    break;
                }
            }
            finally
            {
                ConsoleTheme.Reset();
            }
        }
    }
}