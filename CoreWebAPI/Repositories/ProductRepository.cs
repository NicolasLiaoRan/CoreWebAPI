using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebAPI.Data;
using CoreWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreWebAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext _myDbContext;

        public ProductRepository(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public Material GetMaterialForProduct(int productId, int materialId)
        {
            return _myDbContext.Materials.FirstOrDefault(x => x.ProductId == productId && x.Id == materialId);
        }

        public IEnumerable<Material> GetMaterialsForProduct(int productId)
        {
            return _myDbContext.Materials.Where(x => x.ProductId == productId).ToList();   
        }

        public Product GetProduct(int productId, bool includeMaterials)
        {
            if(includeMaterials)
            {
                return _myDbContext.Products.Include(x => x.Materials).FirstOrDefault(x => x.Id == productId);
            }
            return _myDbContext.Products.Find(productId);
        }

        public IEnumerable<Product> GetProducts()
        {
            return _myDbContext.Products.OrderBy(x => x.Name).ToList();
        }
        //新增(但并不持久化到数据库)
        public void AddProduct(Product product)
        {
            _myDbContext.Products.Add(product);
        }
        public bool Save()
        {
            return _myDbContext.SaveChanges() >= 0;
        }

        public void DeleteProduct(Product product)
        {
            _myDbContext.Products.Remove(product);
        }
    }
}
