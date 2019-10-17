using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        //为子属性添加一个导航属性
        public ICollection<Material> Materials { get; set; }
    }
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
