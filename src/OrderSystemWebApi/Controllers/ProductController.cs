using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderSystemWebApi.DTO.Product;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Interfaces.QueryInterfaces;
using OrderSystemWebApi.Mapper;
using OrderSystemWebApi.Query.ProductQuery;

namespace OrderSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepositoryService _productService;
        private readonly IProblemService _problemService;
        private readonly ILoggerService _logger;

        public ProductController(IProductRepositoryService productService, IProblemService problemService, ILoggerService logger)
        {
            _productService = productService;
            _problemService = problemService;
            _logger = logger;
        }


        /// <summary>
        /// Retrieves all products based on query options and sort options.
        /// </summary>
        /// <returns>A list of products.</returns>
        /// <remarks>
        /// This endpoint allows an authenticated user to retrieve all products based on the provided query and sort options.
        /// If the request is successful, a 200 OK response is returned with the list of products.
        /// Authorization required.
        /// </remarks>
        /// <response code="200">Returns the list of products.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadProductRequestDTO>>> GetAll([FromQuery] ProductQueryOptions options)
        {
            await _logger.LogRequestInformation(Request, "Get all product request.");
            try 
            {
                return await GetAllAsync(options);
            }
            catch (Exception e)
            {
                return StatusCode(500, _problemService.CreateInternalServerErrorProblemDetails([e.Message], Request.Path));
            }
        }

        private async Task<ActionResult<IEnumerable<ReadProductRequestDTO>>> GetAllAsync(ProductQueryOptions options)
        {
            var products = _productService.GetAll(options);

            var filteredProducts = products.Select(p => p.ToReadProductDTO());

            return await Task.FromResult(Ok(filteredProducts));
        }

        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="Id">The ID of the product.</param>
        /// <returns>The product with the specified ID.</returns>
        /// <remarks>
        /// This endpoint allows an authenticated user to retrieve a product by its ID.
        /// If the product is not found, a 404 Not Found response is returned.
        /// If the request is successful, a 200 OK response is returned with the product details.
        /// Authorization Required
        /// </remarks>
        /// <response code="200">Returns the product with the specified ID.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="404">If the product is not found.</response>
        [Authorize]
        [HttpGet("{Id:guid}")]
        public async Task<ActionResult<ReadProductRequestDTO>> GetById(Guid Id)
        {
            var product = await _productService.GetByIdAsync(Id);

            if (product == null)
                return NotFound(_problemService.CreateNotFoundProblemDetails("Id not found.", Request.Path));

            return product.ToReadProductDTO();
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="request">The product data to create.</param>
        /// <returns>The created product.</returns>
        /// <remarks>
        /// This endpoint allows users with the Admin or Moderator role to create a new product.
        /// If the product data is invalid, a 400 Bad Request response is returned.
        /// If the request is successful, a 201 Created response is returned with the newly created product.
        /// Authorization required with a role of admin or moderator.
        /// </remarks>
        /// <response code="201">Returns the newly created product.</response>
        /// <response code="400">If the product data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is authenticated but does not have the admin or moderator role.</response>
        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public async Task<ActionResult<ReadProductRequestDTO>> Create([FromBody] WriteProductRequestDTO request)
        {
            var product = await _productService.CreateProduct(request);

            return CreatedAtAction(nameof(GetById), new { product.Id }, product.ToReadProductDTO());
        }

        /// <summary>
        /// Updates an existing product by its ID.
        /// </summary>
        /// <param name="Id">The ID of the product to update.</param>
        /// <param name="request">The updated product data.</param>
        /// <returns>A status indicating the result of the update operation.</returns>
        /// <remarks>
        /// This endpoint allows users with the Admin or Moderator role to update an existing product by its ID.
        /// If the product is not found, a 404 Not Found response is returned.
        /// If the product data is invalid, a 400 Bad Request response is returned.
        /// If the request is successful, a 200 OK response is returned indicating the update was successful.
        /// Authorization required with a role of admin or moderator.
        /// </remarks>
        /// <response code="200">Product operation completed successfully.</response>
        /// <response code="404">If the product is not found.</response>
        /// <response code="400">If the product data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is authenticated but does not have the admin or moderator role.</response>
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

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="Id">The ID of the product to delete.</param>
        /// <returns>A status indicating the result of the delete operation.</returns>
        /// <remarks>
        /// This endpoint allows users with the Admin or Moderator role to delete a product by its ID.
        /// If the product is not found, a 404 Not Found response is returned.
        /// If the request is successful, a 200 OK response is returned indicating the product was deleted successfully.
        /// Authorization is required with a role of admin or moderator.
        /// </remarks>
        /// <response code="200">Product deleted successfully.</response>
        /// <response code="404">If the product is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is authenticated but does not have the admin or moderator role.</response>
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
