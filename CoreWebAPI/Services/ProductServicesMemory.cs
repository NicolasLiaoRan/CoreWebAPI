using CoreWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.Services
{
    public class ProductServicesMemory
    {
        public static ProductServicesMemory productServices { get; } = new ProductServicesMemory();
        public List<ProductMemory> products { get; }

        private ProductServicesMemory()
        {
            products = new List<ProductMemory>
            {
                new ProductMemory
                {
                    Id=1,
                    Name="牛奶",
                    Price=new decimal(2.5),
                    Materials=new List<MaterialMemory>
                    {
                        new MaterialMemory
                        {
                            Id = 1,
                            Name = "水"
                        }, 
                        new MaterialMemory
                        {
                            Id = 2,
                            Name = "奶粉"
                        }
                    }
                },
                new ProductMemory
                {
                    Id=2,Name="猪肉",
                    Price=new decimal(12.5),
                    Materials=new List<MaterialMemory>
                    {
                        new MaterialMemory
                        {
                            Id = 3,
                            Name = "面粉"
                        },
                        new MaterialMemory
                        {
                            Id = 4,
                            Name = "糖"
                        }
                    }
                },
                new ProductMemory
                {
                    Id=3,
                    Name="鸡蛋",
                    Price=new decimal(1.5),
                    Materials=new List<MaterialMemory>
                    {
                        new MaterialMemory
                        {
                            Id = 5,
                            Name = "麦芽"
                        },
                        new MaterialMemory
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
