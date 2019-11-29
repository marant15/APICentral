using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Services;
using Services.Models;

namespace APICentral.Controllers
{
    [Route("crm-api/products")]
    public class ProductController : Controller
    {
        private IProductService _productService;
        private IConfiguration _configuration;

        public ProductController(IConfiguration configuration, IProductService productService)
        {
            _configuration = configuration;
            _productService = productService;
        }

        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_productService.GetAllProductsAsync().Result);
        }

        [HttpGet]
        [Route("list")]
        public IActionResult Get([FromQuery]string ids)
        {
            return Ok(_productService.GetSomeProductsAsync(ids).Result);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult GetOne(String id)
        {
            return Ok(_productService.GetProductAsync(id).Result);
        }

        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody]ProductPostDTO product)
        {
            bool status = _productService.CreateAsync(product).Result;
            if (status)
            {
                return Ok();
            }
            else
            {
                return BadRequest("error");
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public IActionResult Put(String id, [FromBody]ProductPutDTO product)
        {
            bool status = _productService.UpdateAsync(product, id).Result;
            if (status)
            {
                return Ok();
            }
            else
            {
                return BadRequest("error");
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(String id)
        {
            bool status = _productService.DeleteAsync(id).Result;
            if (status)
            {
                return Ok();
            }
            else
            {
                return BadRequest("error");
            }
        }
    }
}
