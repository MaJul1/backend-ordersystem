using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderSystemWebApi.DTO;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Mapper;
using OrderSystemWebApi.Repository;

namespace OrderSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepositoryService _productService;

        public ProductController(IProductRepositoryService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadProductRequestDTO>>> GetAll()
        {
            var products = _productService.GetAll();

            var filteredProducts = products.Select(p => p.ToReadProductDTO());

            return await Task.FromResult(Ok(filteredProducts));
        }

        [HttpGet("{Id:guid}")]
        public async Task<ActionResult<ReadProductRequestDTO>> GetById(Guid Id)
        {
            var product = await _productService.GetByIdAsync(Id);

            if (product == null)
                return NotFound("Id not found");

            return product.ToReadProductDTO();
        }

        [HttpPost]
        public async Task<ActionResult<ReadProductRequestDTO>> Create([FromBody] WriteProductRequestDTO request)
        {
            var product = await _productService.CreateProduct(request);

            return CreatedAtAction(nameof(GetById), new { product.Id }, product.ToReadProductDTO());
        }

        [HttpPut("{Id:guid}")]
        public async Task<IActionResult> Update(Guid Id, [FromBody] WriteProductRequestDTO request)
        {
            var product = await _productService.GetByIdAsync(Id);

            if (product == null)
                return NotFound("Id not found");

            await _productService.UpdateProductAsync(Id, request);

            return Ok("Product operation completed successfully.");
        }

        [HttpDelete("{Id:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid Id)
        {
            var product = await _productService.GetByIdAsync(Id);

            if (product == null)
                return NotFound("Id not found");

            await _productService.DeleteProductAsync(Id);

            return Ok("Product operation completed successfully.");
        }
    }
}
 