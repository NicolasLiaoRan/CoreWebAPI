using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebAPI.Models;
using CoreWebAPI.Models.ViewModels;
using CoreWebAPI.Repositories;
using CoreWebAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreWebAPI.Controllers
{
    //配置路由
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IMailService _imailService;
        private readonly IProductRepository _productRepository;

        //Ioc+DI示例：通过构造函数注入
        public ProductController(ILogger<ProductController> logger, IMailService imailService, IProductRepository productRepository)
        {
            _logger = logger;
            _imailService = imailService;
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetProduct()
        {
            //return Ok(ProductServicesMemory.productServices.products);
            var products = _productRepository.GetProducts();
            var results = new List<ProductWithoutMaterialMemory>();
            foreach (var item in products)
            {
                results.Add(new ProductWithoutMaterialMemory
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Describer = item.Describer
                });
            }
            return Ok(results);
        }
        [HttpGet]
        [Route("{id}", Name = "GetProduct")]
        public IActionResult GetProduct(int id, bool includeMaterial = false)
        {
            //var product = ProductServicesMemory.productServices.products.Find(x => x.Id == id);
            //if (product == null)
            //{
            //    //日志记录
            //    _logger.LogInformation($"Id为{id}的产品没有找到..");
            //    return NotFound();
            //}
            //return Ok(product);
            var product = _productRepository.GetProduct(id, includeMaterial);
            if (product == null)
            {
                _logger.LogInformation($"Id为{id}的产品没有找到..");
                return NotFound();
            }
            //如果带有子model material,注意查询的是单个product
            if (includeMaterial)
            {
                var productWithMaterial = new ProductMemory
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Describer = product.Describer
                };
                foreach (var item in product.Materials)
                {
                    productWithMaterial.Materials.Add(new MaterialMemory
                    {
                        Id = item.Id,
                        Name = item.Name
                    });
                }
                return Ok(productWithMaterial);
            }
            //如果不带有子model material
            var onlyProduct = new ProductMemory
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Describer = product.Describer
            };
            return Ok(onlyProduct);
        }
        [HttpPost]
        public IActionResult Post([FromBody]ProductViewModel productViewModel)
        {
            if (productViewModel == null)
                return BadRequest();
            //添加Data Annotation对应的Model验证
            if (productViewModel.Name == "产品")
                ModelState.AddModelError("Name", "名称不可以为产品");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var maxId = ProductServicesMemory.productServices.products.Max(x => x.Id);
            var newProduct = new ProductMemory
            {
                Id = ++maxId,
                Name = productViewModel.Name,
                Price = productViewModel.Price
            };
            ProductServicesMemory.productServices.products.Add(newProduct);
            return CreatedAtRoute("GetProduct", new { id = newProduct.Id }, newProduct);
        }
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] ProductModificationViewModel productModificationViewModel, int id)
        {
            if (productModificationViewModel == null)
                return BadRequest();
            if (productModificationViewModel.Name == "产品")
                ModelState.AddModelError("Name", "名称不可以是产品");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var model = ProductServicesMemory.productServices.products.Find(x => x.Id == id);
            if (model == null)
                return NotFound();
            model.Name = productModificationViewModel.Name;
            model.Price = productModificationViewModel.Price;
            return NoContent();
        }
        [HttpPatch("{id}")]
        public IActionResult Patch([FromBody]JsonPatchDocument<ProductModificationViewModel> patchDocument, int id)
        {
            if (patchDocument == null)
                return BadRequest();
            var model = ProductServicesMemory.productServices.products.Find(x => x.Id == id);
            if (model == null)
                return NotFound();
            var patch = new ProductModificationViewModel
            {
                Name = model.Name,
                Price = model.Price
            };
            patchDocument.ApplyTo(patch, ModelState);
            //因为传入的是JsonPatchDocument,所以需要再检验一次
            if (patch.Name == "产品")
                ModelState.AddModelError("Name", "名称不可以是产品");
            TryValidateModel(patch);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            model.Name = patch.Name;
            model.Price = patch.Price;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var model = ProductServicesMemory.productServices.products.Find(x => x.Id == id);
            if (model == null)
                return NotFound();
            ProductServicesMemory.productServices.products.Remove(model);
            //在这里就可以调用自定义服务
            _imailService.Send("product deleted", $"Id为{id}的产品被删除了");
            return NoContent();
        }
    }
}