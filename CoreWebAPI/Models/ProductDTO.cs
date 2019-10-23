using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.Models
{
    public class ProductDTO
    {
        public ProductDTO()
        {
            Materials = new List<MaterialDTO>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Describer { get; set; }
        //为子属性添加一个导航属性
        public ICollection<MaterialDTO> Materials { get; set; }
        public int MaterialCount => Materials.Count;
    }
}
