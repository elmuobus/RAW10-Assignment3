using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Assignment3
{ 
    public struct CategoryRow 
    {
        [JsonPropertyName("cid")]
        public int? Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
    public class Category
    {
        private static List<CategoryRow> _data = new List<CategoryRow>()
        {
            new CategoryRow()
            {
                Id = 1,
                Name = "Beverages",
            },
            new CategoryRow()
            {
                Id = 2,
                Name = "Condiments",
            },
            new CategoryRow()
            {
                Id = 3,
                Name = "Confections",
            },
        };

        public static CategoryRow AddNewName(string name)
        {
            var item = new CategoryRow()
            {
                Id = _data.Count + 1,
                Name = name,
            };
            
            _data.Add(item);
            return item;
        }

        public static List<CategoryRow> ReadAll()
        {
            return _data;
        }

        public static CategoryRow? ReadOne(int cid)
        {
            try
            {
                return _data[cid - 1];
            }
            catch (ArgumentOutOfRangeException e)
            {
                return null;
            }
        }
        
        public static string Update(int cid, CategoryRow newCategory) 
        { 
            try 
            { 
                _data[cid - 1] = newCategory; 
                return "3 Updated";
            }
            catch (ArgumentOutOfRangeException e) 
            { 
                return "5 Not Found";
            }
        }
        
        public static string Remove(int cid) 
        { 
            try 
            { 
                _data.RemoveAt(cid - 1); 
                return "1 Ok";
            }
            catch (ArgumentOutOfRangeException e) 
            { 
                return "5 Not Found";
            }
        }
    }
}