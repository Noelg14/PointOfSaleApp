using HOApi.Models;
using HOApi.Repository;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Text.RegularExpressions;
using Newtonsoft.Json;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HOApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // GET: api/<ProductController>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Get()
        {
            IEnumerable<Product> products = ProductRepo.getProducts();
            if(products.Any() == false)
            {
                return BadRequest();
            }
            else
            {
                return Ok(products);
            }

        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            Product p = ProductRepo.getProductByPLU(id);
            if( p.PLU != id)
            {
                return BadRequest("PLU not found");
            }
            return Ok(p);
        }

        // POST api/<ProductController>
        [HttpPost]
        // have this as 1 call? IEnum<Product> or as indiviudual calls?
        //Both?
        public void Post([FromBody] string value)
        {
        }

        // POST api/<ProductController>
        [HttpPost("allproducts")]
        // have this as 1 call? IEnum<Product> or as indiviudual calls?
        //Both?
        public IActionResult allProducts([FromBody] List<Product> values)
        {
            foreach(Product newProduct in values )
            {
                if (!ProductRepo.createProduct(newProduct))
                {
                    return StatusCode(500, "Error creating product");
                }
            }
            return Ok();
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        //// DELETE api/<ProductController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
