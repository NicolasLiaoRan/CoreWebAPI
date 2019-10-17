using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebAPI.Models;
using CoreWebAPI.Models.ViewModels;
using CoreWebAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebAPI.Controllers
{
    //配置路由
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        [HttpGet]
        public IActionResult GetProduct()
        {
            return Ok(ProductServices.productServices.products);
        }
        [HttpGet]
        [Route("{id}",Name ="GetProduct")]
        public IActionResult GetProduct(int id)
        {
            var product = ProductServices.productServices.products.Find(x => x.Id == id);
          if (product == null)
                return NotFound();
            return Ok(product);
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
            var maxId = ProductServices.productServices.products.Max(x => x.Id);
            var newProduct = new Product
            {
                Id=++maxId,
                Name=productViewModel.Name,
                Price=productViewModel.Price
            };
            ProductServices.productServices.products.Add(newProduct);
            return CreatedAtRoute("GetProduct", new { id = newProduct.Id }, newProduct);
        }
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] ProductModificationViewModel productModificationViewModel,int id)
        {
            if (productModificationViewModel == null)
                return BadRequest();
            if (productModificationViewModel.Name == "产品")
                ModelState.AddModelError("Name", "名称不可以是产品");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var model = ProductServices.productServices.products.Find(x => x.Id == id);
            if (model == null)
                return NotFound();
            model.Name = productModificationViewModel.Name;
            model.Price = productModificationViewModel.Price;
            return NoContent();
        }
        [HttpPatch("{id}")]
        public IActionResult Patch([FromBody]JsonPatchDocument<ProductModificationViewModel> patchDocument,int id)
        {
            if (patchDocument == null)
                return BadRequest();
            var model = ProductServices.productServices.products.Find(x => x.Id == id);
            if (model == null)
                return NotFound();
            var patch = new ProductModificationViewModel
            {
                Name=model.Name,
                Price=model.Price
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
            var model = ProductServices.productServices.products.Find(x => x.Id == id);
            if (model == null)
                return NotFound();
            ProductServices.productServices.products.Remove(model);
            return NoContent();
        }
    }
}