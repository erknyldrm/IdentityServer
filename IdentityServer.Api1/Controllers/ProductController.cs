using IdentityServer.Api1.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Api1.Controllers
{
    [Route("api/[controller][action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [Authorize(Policy = "ReadProduct")]
        [HttpGet]
        public IActionResult GetProducts()
        {
            List<Product> products = new() {
                new Product { Id = 1, Name = "Kalem", Price = 100, Stock = 500 },
                new Product { Id = 2, Name = "Silgi", Price = 100, Stock = 500 },
                new Product { Id = 3, Name = "Defter", Price = 100, Stock = 500 },
                new Product { Id = 4, Name = "Kitap", Price = 100, Stock = 500 },
                new Product { Id = 5, Name = "Bant", Price = 100, Stock = 500 }
            };

            return Ok(products);
        }

        [Authorize(Policy ="CreateOrUpdate")]
        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            return Ok($"id si {id} olan ürün güncellendi.");
        }

        [HttpPost]
        [Authorize(Policy = "CreateOrUpdate")]
        public IActionResult CreateProduct(Product product)
        {
            return Ok(product.Id);
        }
    }
}
