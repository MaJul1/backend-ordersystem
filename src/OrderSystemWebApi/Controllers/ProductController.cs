using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderSystemWebApi.DTO.Product;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Mapper;
using OrderSystemWebApi.Query;

namespace OrderSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepositoryService _productService;
        private readonly IProblemService _problemService;

        public ProductController(IProductRepositoryService productService, IProblemService problemService)
        {
            _productService = productService;
            _problemService = problemService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadProductRequestDTO>>> GetAll
        (
            [FromQuery] double? maximumPrice,
            [FromQuery] double? minimumPrice,
            [FromQuery] string? orderByPropertyName,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            [FromQuery] bool? isDescending
        )
        {
            ProductQueryOptions queryOptions = new()
            {
                MaximumPrice = maximumPrice,
                MinimumPrice = minimumPrice,
                OrderByPropertyName = orderByPropertyName,
                IsDescending = isDescending ?? false,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var products = _productService.GetAll();

            var queriedProducts = products.ApplyQueryService(queryOptions);

            var filteredProducts = queriedProducts.Select(p => p.ToReadProductDTO());

            return await Task.FromResult(Ok(filteredProducts));
        }

        [Authorize]
        [HttpGet("{Id:guid}")]
        public async Task<ActionResult<ReadProductRequestDTO>> GetById (Guid Id)
        {
            var product = await _productService.GetByIdAsync(Id);

            if (product == null)
                return NotFound(_problemService.CreateNotFoundProblemDetails("Id not found.", Request.Path));

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
                return NotFound(_problemService.CreateNotFoundProblemDetails("Id not found.", Request.Path));

            await _productService.UpdateProductAsync(Id, request);

            return Ok("Product operation completed successfully.");
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpDelete("{Id:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid Id)
        {
            var product = await _productService.GetByIdAsync(Id);

            if (product == null)
                return NotFound(_problemService.CreateNotFoundProblemDetails("Id not found.", Request.Path));

            await _productService.DeleteProductAsync(Id);

            return Ok("Product deleted.");
        }
    }
}
 