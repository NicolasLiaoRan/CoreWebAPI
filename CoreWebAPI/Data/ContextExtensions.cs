using CoreWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.Data
{
    public static class ContextExtensions
    {
        public static void SeedForContext(this MyDbContext myDbContext)
        {
            if (myDbContext.Products.Any())
                return;
            var products = new List<Product>
            {
                new Product
                {
                    Name = "华为P30",
                    Price = new decimal(5000),
                    Describer = "2019-10"
                },
                new Product
                {
                    Name = "OPPO Reno",
                    Price = new decimal(4000),
                    Describer = "2019-09"
                },
                new Product
                {
                    Name = "三星S10",
                    Price = new decimal(6000),
                    Describer = "2019-06"
                },
                new Product
                {
                    Name = "小米9",
                    Price = new decimal(3500),
                    Describer = "2019-05",
                    Materials=new List<Material>
                    {
                        new Material
                        {
                            Name="SuperAMOLED"
                        },
                        new Material
                        {
                            Name="Snapdragon 855plus"
                        }
                    }
                },
                new Product
                {
                    Name = "魅族16t",
                    Price = new decimal(4500),
                    Describer = "2019-03",
                    Materials=new List<Material>
                    {
                        new Material
                        {
                            Name="IPS"
                        },
                        new Material
                        {
                            Name="Snapdragon 855"
                        }
                    }
                }

            };
            myDbContext.Products.AddRange(products);
            myDbContext.SaveChanges();
        }
    }
}
