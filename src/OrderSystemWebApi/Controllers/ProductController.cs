using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderSystemWebApi.DTO.Product;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Mapper;

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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadProductRequestDTO>>> GetAll()
        {
            var products = _productService.GetAll();

            var filteredProducts = products.Select(p => p.ToReadProductDTO());

            return await Task.FromResult(Ok(filteredProducts));
        }

        [Authorize]
        [HttpGet("{Id:guid}")]
        public async Task<ActionResult<ReadProductRequestDTO>> GetById(Guid Id)
        {
            var product = await _productService.GetByIdAsync(Id);

            if (product == null)
                return NotFound("Id not found");

            return product.ToReadProductDTO();
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public async Task<ActionResult<ReadProductRequestDTO>> Create([FromBody] WriteProductRequestDTO request)
        {
            var product = await _productService.CreateProduct(request);

            return CreatedAtAction(nameof(GetById), new { product.Id }, product.ToReadProductDTO());
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPut("{Id:guid}")]
        public async Task<IActionResult> Update(Guid Id, [FromBody] WriteProductRequestDTO request)
        {
            var product = await _productService.GetByIdAsync(Id);

            if (product == null)
                return NotFound("Id not found");

            await _productService.UpdateProductAsync(Id, request);

            return Ok("Product operation completed successfully.");
        }

        [Authorize(Roles = "Admin,Moderator")]
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
 