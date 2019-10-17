using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebAPI.Models;
using CoreWebAPI.Services;
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
        [Route("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = ProductServices.productServices.products.Find(x => x.Id == id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }
    }
}