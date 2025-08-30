using _28._08MiniProject.Models.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _28._08MiniProject.Repositories
{
    internal class Repository
    {
        public void Serialize<T>(List<T> items, string path)
        {
            try
            {
                string json = JsonConvert.SerializeObject(items, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serialization: {ex.Message}");
            }
        }
        public List<T> Deserialize<T>(string path) where T : BaseEntity
        {
            string result = null;
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    result = sr.ReadToEnd();
                }
                List<T> items = null;
                if (string.IsNullOrEmpty(result))
                {
                    items = new List<T>();
                }
                else
                {
                    items = JsonConvert.DeserializeObject<List<T>>(result);
                }
                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserialization: {ex.Message}");
                return new List<T>();  
            }
        }
    }
}
