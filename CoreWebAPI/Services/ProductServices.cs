using CoreWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.Services
{
    public class ProductServices
    {
        public static ProductServices productServices { get; } = new ProductServices();
        public List<Product> products { get; }

        private ProductServices()
        {
            products = new List<Product>
            {
                new Product
                {
                    Id=1,
                    Name="牛奶",
                    Price=2.5f,
                    Materials=new List<Material>
                    {
                        new Material
                        {
                            Id = 1,
                            Name = "水"
                        }, 
                        new Material
                        {
                            Id = 2,
                            Name = "奶粉"
                        }
                    }
                },
                new Product
                {
                    Id=2,Name="猪肉",
                    Price=12.5f,
                    Materials=new List<Material>
                    {
                        new Material
                        {
                            Id = 3,
                            Name = "面粉"
                        },
                        new Material
                        {
                            Id = 4,
                            Name = "糖"
                        }
                    }
                },
                new Product
                {
                    Id=3,
                    Name="鸡蛋",
                    Price=2.4f,
                    Materials=new List<Material>
                    {
                        new Material
                        {
                            Id = 5,
                            Name = "麦芽"
                        },
                        new Material
                        {
                            Id = 6,
                            Name = "地下水"
                        }
                    }
                }
            };
        }
    }
}
