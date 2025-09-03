using _28._08MiniProject.Models;
using _28._08MiniProject.Repositories;
using _28._08MiniProject.Utilities.VisualEffect;
namespace _28._08MiniProject.Services
{
    internal class ProductServices
    {
        private readonly static string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Data", "Product.json");
    
        public Repository ProductRepository { get; set; } = new Repository();
        public void CreateProduct()
        {
            ConsoleTheme.SetSubmenu(); 
            try
            {
                Console.WriteLine("PRODUCT MENU");
                Console.WriteLine("Please input product's info:");
                var products = ReadProduct();

                string? name = null;
                decimal price = 0;
                double stock = 0;
                int step = 1;

                while (step <= 3)
                {
                    if (step == 1)
                    {
                        Console.WriteLine("Write 'menu' = back to menu \nPlease input product's name(min 1 char):");
                        var input = (Console.ReadLine() ?? "").Trim();
                        if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;

                        if (string.IsNullOrWhiteSpace(input))
                        {
                            Console.Clear();
                            ConsoleTheme.WriteError("Name must be minimum one character.");
                            continue;
                        }

                        bool exists = products.Any(p => p.Name != null &&
                                                        p.Name.Equals(input, StringComparison.OrdinalIgnoreCase));
                        if (exists)
                        {
                            Console.Clear();
                            ConsoleTheme.WriteError("This product name already exists. Try again.");
                            continue;
                        }
                        name = input.ToUpperInvariant();
                        step++;
                    }
                    else if (step == 2)
                    {
                        Console.WriteLine("Write 'back' = go back, 'menu' = back to menu\nPlease input product's price (> 0): ");
                        var input = (Console.ReadLine() ?? "").Trim();
                        if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;
                        if (input.Equals("back", StringComparison.OrdinalIgnoreCase)) { step--; continue; }

                        if (!decimal.TryParse(input, out price) || price <= 0)
                        {
                            Console.Clear();
                            ConsoleTheme.WriteError("Price must be > 0.");
                            continue;
                        }
                        step++;
                    }
                    else
                    {
                        Console.WriteLine("Write 'back' = go back, 'menu' = back to menu\nPlease input product's stock (>= 0):");
                        var input = (Console.ReadLine() ?? "").Trim();
                        if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;
                        if (input.Equals("back", StringComparison.OrdinalIgnoreCase)) { step--; continue; }

                        if (!double.TryParse(input, out stock) || stock < 0)
                        {
                            Console.Clear();
                            ConsoleTheme.WriteError("Stock must be >= 0.");
                            continue;
                        }
                        step++;
                    }
                }

                var product = new Product(name!, price, stock);
                products.Add(product);
                WriteProduct(products);
                Console.WriteLine("Product Created");
                Console.ReadKey();
            }
            finally
            {
                ConsoleTheme.Reset(); 
            }
        }
        public void DeleteProduct()
        {
            ConsoleTheme.SetSubmenu();
            try
            {
                var products = ReadProduct();
                if (products.Count == 0)
                {
                    Console.Clear();
                    ConsoleTheme.WriteError("No products found.");
                    return;
                }
                int step = 0;
                Guid id;
                while (true)
                {
                    if (step == 0)
                    {
                        Console.WriteLine("PRODUCT MENU");
                        Console.WriteLine(new string('-', 80));
                        ShowAllProduct(false);
                        Console.WriteLine(new string('-', 80));
                        Console.WriteLine("Commands: list = show again, menu = back to menu");
                        step = 1;
                    }
                    else if (step == 1)
                    {
                        Console.Write("Enter product Id (or 'list', 'menu'): ");
                        var input = (Console.ReadLine() ?? "").Trim();
                        if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;
                        if (input.Equals("list", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine(new string('-', 80));
                            ShowAllProduct(false);
                            Console.WriteLine(new string('-', 80));
                            continue;
                        }
                        if (!Guid.TryParse(input, out id))
                        {
                            Console.Clear();
                            ConsoleTheme.WriteError("Wrong Id. Try again.");
                            continue;
                        }
                        var product = products.FirstOrDefault(p => p.Id == id);
                        if (product == null)
                        {
                            Console.Clear();
                            ConsoleTheme.WriteError("Product not found. Try again.");
                            continue;
                        }
                        products.Remove(product);
                        WriteProduct(products);
                        Console.WriteLine($"Product '{product.Name}' successfully deleted.");
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
        public void GetProductById()
        {
            ConsoleTheme.SetSubmenu();
            try
            {
                Console.WriteLine("ORDER MENU");
                Console.WriteLine("Write 'menu' to go menu");
                Console.Write("Please input product's id for product's info: ");
                while (true)
                {
                    string input = (Console.ReadLine() ?? "").Trim();

                    if (input.Equals("menu", StringComparison.OrdinalIgnoreCase))
                        return;

                    if (!Guid.TryParse(input, out Guid id))
                    {
                        ConsoleTheme.WriteError("Wrong Id. Please input again:");
                        continue;
                    }
                    var products = ReadProduct();
                    var product = products.FirstOrDefault(p => p.Id == id);
                    if (product == null)
                    {
                        ConsoleTheme.WriteError("Product not found. Input again:");
                        continue;
                    }
                    Console.WriteLine();
                    Console.WriteLine(new string('-', 85));
                    product.PrintInfo();
                    Console.WriteLine(new string('-', 85));
                    break; 
                }
                Console.ReadKey();
            }
            finally
            {
                ConsoleTheme.Reset(); 
            }
        }
        public void ShowAllProduct(bool pause = true)
        {
            ConsoleTheme.SetSubmenu();
            try
            {
                Console.WriteLine("PRODUCT MENU");

                var products = ReadProduct();
                if (products.Count == 0)
                {
                    ConsoleTheme.WriteError("You have no product.");
                    return;
                }
                Console.WriteLine("All Product's Info:");
                foreach (var product in products)
                {
                    string stockStatus = product.Stock > 0
                        ? $"Stock: {product.Stock}"
                        : "Out of Stock";  
                    Console.WriteLine(
                        $"Id: {product.Id}, Product: {product.Name}, Price: {product.Price:C}, {stockStatus}"
                    );
                    Console.WriteLine(new string('-', 80));
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
        public void RefillProduct()
        {
            ConsoleTheme.SetSubmenu();
            try
            {
                var products = ReadProduct();
                if (products.Count == 0)
                {
                    Console.Clear();
                    ConsoleTheme.WriteError("No products found.");
                    return;
                }
                Console.WriteLine(new string('-', 80));
                ShowAllProduct(false);
                Console.WriteLine(new string('-', 80));
                Guid id;
                Product? productToRefill = null;
                while (true)
                {
                    Console.Write("Write 'menu' to go menu, 'list' to show products again.\nPlease input product Id to refill: ");
                    var inputId = (Console.ReadLine() ?? "").Trim();
                    if (inputId.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;
                    if (inputId.Equals("list", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.Clear();
                        Console.WriteLine(new string('-', 80));
                        ShowAllProduct(false);
                        Console.WriteLine(new string('-', 80));
                        continue;
                    }
                    if (!Guid.TryParse(inputId, out id))
                    {
                        Console.Clear();
                        ConsoleTheme.WriteError("Wrong Id. Please enter a valid product ID.");
                        continue;
                    }
                    productToRefill = products.FirstOrDefault(p => p.Id == id);
                    if (productToRefill == null)
                    {
                        Console.Clear();
                        ConsoleTheme.WriteError("Product not found. Try again.");
                        continue;
                    }
                    break;
                }
                while (true)
                {
                    Console.WriteLine(
                        $"Write 'back' to choose another product, 'menu' to go menu\n" +
                        $"Please input the amount to refill (negative => decrease, positive => increase)\n" +
                        $"Current stock: {productToRefill!.Stock}"
                    );
                    var refillInput = (Console.ReadLine() ?? "").Trim();
                    if (refillInput.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;
                    if (refillInput.Equals("back", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine(new string('-', 80));
                        ShowAllProduct(false);
                        Console.WriteLine(new string('-', 80));
                        productToRefill = null;
                        while (true)
                        {
                            Console.Write("Input product Id: ");
                            var idStr = (Console.ReadLine() ?? "").Trim();
                            if (idStr.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;
                            if (!Guid.TryParse(idStr, out id))
                            {
                                Console.Clear();
                                ConsoleTheme.WriteError("Wrong Id. Please enter a valid product ID.");
                                continue;
                            }
                            productToRefill = products.FirstOrDefault(p => p.Id == id);
                            if (productToRefill == null)
                            {
                                Console.Clear();
                                ConsoleTheme.WriteError("Product not found. Try again.");
                                continue;
                            }
                            break;
                        }
                        continue;
                    }
                    if (!double.TryParse(refillInput, out double refillAmount))
                    {
                        Console.Clear();
                        ConsoleTheme.WriteError("Please input a valid number.");
                        continue;
                    }
                    if (refillAmount == 0)
                    {
                        Console.Clear();
                        ConsoleTheme.WriteError("Input can't be 0. Try again.");
                        continue;
                    }
                    if (productToRefill.Stock + refillAmount < 0)
                    {
                        Console.Clear();
                        ConsoleTheme.WriteError($"Stock can't be negative. Current stock: {productToRefill.Stock}");
                        continue;
                    }
                    productToRefill.Stock += refillAmount;
                    WriteProduct(products);
                    Console.WriteLine($"{productToRefill.Name} stock updated. New stock: {productToRefill.Stock}");
                    Console.ReadKey();
                    break;
                }
            }
            finally
            {
                ConsoleTheme.Reset();
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