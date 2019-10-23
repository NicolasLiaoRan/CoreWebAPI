using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreWebAPI.Services;
using CoreWebAPI.Repositories;
using CoreWebAPI.Models;
using Microsoft.Extensions.Logging;

namespace CoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class MaterialController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<MaterialController> _logger;

        public MaterialController(IProductRepository productRepository,ILogger<MaterialController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult GetMaterial()
        {
            //这里使用的还是老的内存数据，不是数据库数据
            var material = ProductServicesMemory.productServices.products;
            if (material == null)
                return NotFound();
            return Ok(material);
        }
        [HttpGet("{productId}")]
        public IActionResult GetMaterials(int productId)
        {
            var materials = _productRepository.GetMaterialsForProduct(productId);
            if (materials == null)
            {
                _logger.LogInformation($"没能找到{productId}对应的Material");
                return NotFound();
            }
            var results = materials.Select(material => new MaterialMemory
            {
                Id=material.Id,
                Name=material.Name
            }).ToList();
            return Ok(results);
        }
        [HttpGet("{productId}/{materialId}")]
        public IActionResult GetMaterialById(int productId,int materialId)
        {
            var material = _productRepository.GetMaterialForProduct(productId, materialId);
            if(material==null)
            {
                _logger.LogInformation($"{productId}对应的{materialId}没有找到");
                return NotFound();
            }
            var result = new MaterialMemory
            {
                Id = material.Id,
                Name = material.Name
            };
            return Ok(result);
        }
    }
}