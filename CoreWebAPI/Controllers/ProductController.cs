using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        /// <summary>
        /// GET-查询所有Product
        /// </summary>
        /// <returns>Product集合</returns>
        [HttpGet]
        public IActionResult GetProduct()
        {
            //return Ok(ProductServicesMemory.productServices.products);
            var products = _productRepository.GetProducts();
            //下面是手动映射
            //var results = new List<ProductWithoutMaterialDTO>();
            //foreach (var item in products)
            //{
            //    results.Add(new ProductWithoutMaterialDTO
            //    {
            //        Id = item.Id,
            //        Name = item.Name,
            //        Price = item.Price,
            //        Describer = item.Describer
            //    });
            //}
            //下面是AutoMapper映射
            var results = Mapper.Map<IEnumerable<ProductWithoutMaterialDTO>>(products);
            return Ok(results);
        }
        /// <summary>
        /// GET-根据id查询对应的单个Product，并根据条件获取其Material子数据
        /// </summary>
        /// <param name="id">ProductId</param>
        /// <param name="includeMaterial">是否查询Material子数据</param>
        /// <returns></returns>
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
                //下面是手动映射
                //var productWithMaterial = new ProductDTO
                //{
                //    Id = product.Id,
                //    Name = product.Name,
                //    Price = product.Price,
                //    Describer = product.Describer
                //};
                //foreach (var item in product.Materials)
                //{
                //    productWithMaterial.Materials.Add(new MaterialDTO
                //    {
                //        Id = item.Id,
                //        Name = item.Name
                //    });
                //}
                //下面是AutoMapper
                var productWithMaterial = Mapper.Map<ProductDTO>(product);
                return Ok(productWithMaterial);
            }
            //如果不带有子model material，手动映射
            //var onlyProduct = new ProductDTO
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    Price = product.Price,
            //    Describer = product.Describer
            //};
            //AutoMapper
            var onlyProduct = Mapper.Map<ProductWithoutMaterialDTO>(product);
            return Ok(onlyProduct);
        }
        /// <summary>
        /// POST-新增单个对象
        /// </summary>
        /// <param name="productViewModel">ProductViewModel</param>
        /// <returns></returns>
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
            //手动映射
            //var maxId = ProductServicesMemory.productServices.products.Max(x => x.Id);
            //var newProduct = new ProductDTO
            //{
            //    Id = ++maxId,
            //    Name = productViewModel.Name,
            //    Price = productViewModel.Price
            //};
            //ProductServicesMemory.productServices.products.Add(newProduct);
            //AutoMapper
            var newProduct = Mapper.Map<Product>(productViewModel);
            _productRepository.AddProduct(newProduct);
            if(!_productRepository.Save())
            {
                _logger.LogInformation($"保存错误");
                return StatusCode(500, "保存错误");
            }
            var dto = Mapper.Map<ProductWithoutMaterialDTO>(newProduct);
            return CreatedAtRoute("GetProduct", new { id = dto.Id }, dto);
        }
        /// <summary>
        /// PUT-完整更新
        /// </summary>
        /// <param name="productModificationViewModel"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] ProductModificationViewModel productModificationViewModel, int id)
        {
            if (productModificationViewModel == null)
                return BadRequest();
            if (productModificationViewModel.Name == "产品")
                ModelState.AddModelError("Name", "名称不可以是产品");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var model = _productRepository.GetProduct(id,false);
            if (model == null)
                return NotFound();
            //Mapper.Map<Product>(productModificationViewModel);//只是映射值，但并没有改变model的状态，所以持久化到数据库仍然没有变化
            Mapper.Map(productModificationViewModel, model);//把第一个参数的值赋值到第二个参数上，并修改第二个参数的状态为modified，使之可以随DbContext持久化到DB
            if (!_productRepository.Save())
                return StatusCode(500, "更新出错");
            //var model = ProductServicesMemory.productServices.products.Find(x => x.Id == id);
            //if (model == null)
            //    return NotFound();
            //model.Name = productModificationViewModel.Name;
            //model.Price = productModificationViewModel.Price;
            return NoContent();
        }
        /// <summary>
        /// Patch-部分更新
        /// </summary>
        /// <param name="patchDocument"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public IActionResult Patch([FromBody]JsonPatchDocument<ProductModificationViewModel> patchDocument, int id)
        {
            /*//旧模式
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
            */
            if (patchDocument == null)
                return BadRequest();
            var productEntity = _productRepository.GetProduct(id, false);
            if (productEntity == null)
                return NotFound();
            var toPatch = Mapper.Map<ProductModificationViewModel>(productEntity);
            patchDocument.ApplyTo(toPatch, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (toPatch.Name == "产品")
                ModelState.AddModelError("Name", "名称不可以是产品");
            TryValidateModel(toPatch);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Mapper.Map(toPatch, productEntity);
            if (!_productRepository.Save())
                return StatusCode(500, "更新出错");
            return NoContent();
        }
        /// <summary>
        /// DELETE-删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            /*旧代码
            var model = ProductServicesMemory.productServices.products.Find(x => x.Id == id);
            if (model == null)
                return NotFound();
            ProductServicesMemory.productServices.products.Remove(model);
            //在这里就可以调用自定义服务
            _imailService.Send("product deleted", $"Id为{id}的产品被删除了");
            return NoContent();
            */
            var model = _productRepository.GetProduct(id, false);
            if (model == null)
                return NotFound();
            _productRepository.DeleteProduct(model);
            if (!_productRepository.Save())
                return StatusCode(500, "删除错误");
            _imailService.Send("product deleted", $"Id为{id}的产品被删除了");
            return NoContent();
        }
    }
}