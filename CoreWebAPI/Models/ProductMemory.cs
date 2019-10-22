using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.Models
{
    public class ProductMemory
    {
        public ProductMemory()
        {
            Materials = new List<MaterialMemory>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Describer { get; set; }
        //为子属性添加一个导航属性
        public ICollection<MaterialMemory> Materials { get; set; }
        public int MaterialCount => Materials.Count;
    }
}
