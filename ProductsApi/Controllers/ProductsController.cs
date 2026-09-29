
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Models.DTOs.Requests;
using ProductsApi.Services.Interfaces;

namespace ProductsApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAllProducts());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _service.GetProductById(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public IActionResult Create(ProductForCreateDto dto)
        {
            try
            {
                var product = _service.CreateProduct(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = product.Id },
                    product);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(
            int id, ProductForUpdateDto dto)
        {
            if (_service.GetProductById(id) == null)
                return NotFound();

            try
            {
                _service.UpdateProduct(id, dto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_service.GetProductById(id) == null)
                return NotFound();

            _service.DeleteProduct(id);

            return NoContent();
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string name)
        {
            return Ok(_service.SearchProductsByName(name));
        }

        [HttpGet("stats")]
        public IActionResult Stats()
        {
            return Ok(_service.GetStats());
        }
    }
}
