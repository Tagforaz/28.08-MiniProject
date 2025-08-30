using _28._08MiniProject.Models;
using _28._08MiniProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace _28._08MiniProject
{
    internal class ManagementApplication
    {
        private ProductServices productServices=new ProductServices();
        public void Run()
        {

            int num = 0;
            string ?str = null;
            bool result = false;


            while (!(num == 0 && result))
            {
                Console.WriteLine("1.Create Product\n2.Delete Product\n3.Get Product By Id\n4.Show All Product\n5.Refill Product\n6.Order Product\n7.Show All Orders\n8.Charge Order Status\n\n0.Quit");
                str = Console.ReadLine();
                Console.Clear();
                result = int.TryParse(str, out num);
                switch (num)
                {
                    case 1:
                        productServices.CreateProduct();
                        break;
                    case 2:
                        productServices.DeleteProduct();
                        break;
                    case 3:
                        productServices.GetProductById();
                        break;
                    case 4:
                        productServices.ShowAllProduct();
                        break;
                    case 5:
                        productServices.RefillProduct();
                        break;
                    case 6:
                        Console.WriteLine("All product: ");
                        break;
                    case 7:
                        Console.WriteLine("Order status changed");
                        break;
                    case 8:
                        break;
                    case 0:
                        break;
                    default:
                        Console.WriteLine("Wrong Input. Please Try Again");
                        break;
                }
            }
        }
    }
    }

