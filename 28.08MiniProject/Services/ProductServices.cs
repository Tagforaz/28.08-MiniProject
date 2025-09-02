using _28._08MiniProject.Models;
using _28._08MiniProject.Repositories;
namespace _28._08MiniProject.Services
{
    internal class ProductServices
    {
        public static string path = @"C:\Users\Tagforaz\Desktop\Pb306\28.08MiniProject\28.08MiniProject\Data\Product.json";
        public Repository ProductRepository { get; set; } = new Repository();
        public void CreateProduct()
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
                        Console.WriteLine("Name must be minimum one character .");
                        continue;
                    }
                    bool exists = products.Any(p => p.Name != null && p.Name.Equals(input, StringComparison.OrdinalIgnoreCase));
                    if (exists)
                    {
                        Console.Clear();
                        Console.WriteLine("This product name already exists. Try again.");
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
                    else if (input.Equals("back", StringComparison.OrdinalIgnoreCase)) { step--; continue; }
                    if (!decimal.TryParse(input, out price) || price <= 0)
                    {
                        Console.Clear();
                        Console.WriteLine("Price must be > 0.");
                        continue;
                    }
                    step++;
                }
                else 
                {
                    Console.WriteLine("Write 'back' = go back, 'menu' = back to menu\nPlease input product's stock (> 0):");
                    var input = (Console.ReadLine() ?? "").Trim();
                    if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;
                    else if (input.Equals("back", StringComparison.OrdinalIgnoreCase)) { step--; continue; }

                    if (!double.TryParse(input, out stock) || stock < 0)
                    {
                        Console.Clear();
                        Console.WriteLine("Stock must be > 0.");
                        continue;
                    }
                    step++;
                }
            }
            var product = new Product(name!, price, stock);
            products.Add(product);
            WriteProduct(products);
            Console.WriteLine("Product Created");
        }
        public void DeleteProduct()
        {
            Console.WriteLine(new string('-', 80));
            ShowAllProduct();
            Console.WriteLine(new string('-', 80));
            Console.WriteLine("Write 'menu' for back to menu\nPlease, input product's id for delete:");
            List<Product> products = ReadProduct();
            if (products.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }
            Guid id;
            while (true)
            {
                string input = Console.ReadLine()?.Trim() ?? "";
                if (input.Equals("menu", StringComparison.OrdinalIgnoreCase))
                    return; 
                if (!Guid.TryParse(input, out id))
                {
                    Console.Clear();
                    Console.WriteLine("Wrong Id.Input again.");
                    Console.WriteLine(new string('-', 80));
                    ShowAllProduct();
                    Console.WriteLine(new string('-', 80));
                    Console.WriteLine("Write 'menu' for back to menu\nPlease, input product's id for delete:");
                    continue;
                }
                var idToDelete = products.FirstOrDefault(p => p.Id == id);
                if (idToDelete == null)
                {
                    Console.Clear();
                    Console.WriteLine("Product not found. Try again.");
                    Console.WriteLine(new string('-', 80));
                    ShowAllProduct();
                    Console.WriteLine(new string('-', 80));
                    Console.WriteLine("Write 'menu' for back to menu\nPlease, input product's id for delete:");
                    continue;
                }
                products.Remove(idToDelete);
                WriteProduct(products);
                Console.WriteLine($"Product '{idToDelete.Name}' successfully deleted.");
                break;
            }
        }
        public void GetProductById()
        {
            Console.WriteLine("ORDER MENU");
            Console.WriteLine("Write 'menu' to go menu\nPlease input product's id for product's info: ");
            while (true)
            {
                string input = (Console.ReadLine() ?? "").Trim();
                if (input.Equals("menu", StringComparison.OrdinalIgnoreCase))
                    return;
                if (!Guid.TryParse(input, out Guid id))
                {
                    Console.Clear();
                    Console.WriteLine("Write 'menu' to go menu.\nWrong Id. Please input again: ");
                    continue;
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
                    Console.WriteLine("Write 'menu' to go menu\nProduct not found. Input again: ");
                }
            }
        }
        public void ShowAllProduct()
        {
            Console.WriteLine("PRODUCT MENU");
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
                Console.WriteLine($"Id: {product.Id}, Product: {product.Name}, Price: {product.Price:C},  {stockStatus}");
            }
        }
        public void RefillProduct()
        {
            while (true)
            {
                Console.WriteLine(new string('-', 80));
                ShowAllProduct();
                Console.WriteLine(new string('-', 80));
                Console.WriteLine("Write 'menu' for back to main menu\nPlease input product's id to refill:  ");
                string inputId = Console.ReadLine()?.Trim() ?? "";
                if (inputId.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;
                if (inputId.Equals("back", StringComparison.OrdinalIgnoreCase)) continue;
                if (!Guid.TryParse(inputId, out Guid id))
                {
                    Console.Clear();
                    Console.WriteLine("Wrong Id. Please enter a valid product ID.");
                    continue;
                }
                var products = ReadProduct();
                var productToRefill = products.FirstOrDefault(p => p.Id == id);
                if (productToRefill == null)
                {
                    Console.Clear();
                    Console.WriteLine("Product not found. Try again.");
                    continue;
                }
                while (true)
                {
                    Console.WriteLine($"Write 'back' for back,write 'menu'for back to menu\nPlease input the amount to refill (negative => decrease, positive => increase)\nCurrent stock: {productToRefill.Stock}\n" );
                    string refillInput = Console.ReadLine()?.Trim() ?? "";

                    if (refillInput.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;
                    if (refillInput.Equals("back", StringComparison.OrdinalIgnoreCase)) break;

                    if (!double.TryParse(refillInput, out double refillAmount))
                    {
                        Console.Clear();
                        Console.WriteLine("Please input a valid number.");
                        continue;
                    }
                    if (refillAmount == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("Input can't be 0.Input again.");
                        continue;
                    }
                    if (productToRefill.Stock + refillAmount < 0)
                    {
                        Console.Clear();
                        Console.WriteLine($"Stock can't be negative.Current stock: {productToRefill.Stock}");
                        continue;
                    }
                    productToRefill.Stock += refillAmount;
                    WriteProduct(products);
                    Console.WriteLine($"{productToRefill.Name} stock refilled.New stock: {productToRefill.Stock}");
                    return; 
                }
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