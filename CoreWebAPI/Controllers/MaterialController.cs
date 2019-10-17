using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreWebAPI.Services;

namespace CoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class MaterialController : Controller
    {
        [HttpGet]
        public IActionResult GetMaterial()
        {
            var material = ProductServices.productServices.products;
            if (material == null)
                return NotFound();
            return Ok(material);
        }
        [HttpGet("{productId}")]
        public IActionResult GetMaterialAll(int productId)
        {
            var material = ProductServices.productServices.products.Find(x => x.Id == productId);
            if (material == null)
                return NotFound();
            return Ok(material);
        }
        [HttpGet("{productId}/{materialId}")]
        public IActionResult GetMaterialById(int productId,int materialId)
        {
            var product = ProductServices.productServices.products.Find(x => x.Id == productId);
            if (product == null)
                return NotFound();
            var material = product.Materials.SingleOrDefault(x => x.Id == materialId);
            if (material == null)
                return NotFound();
            return Ok(material);
        }
    }
}