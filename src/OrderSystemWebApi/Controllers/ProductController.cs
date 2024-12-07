using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderSystemWebApi.DTO;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Repository;

namespace OrderSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepositoryService _productService;

        public ProductController (IProductRepositoryService productService)
        {
            _productService = productService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadProductRequestDTO>>> GetAll()
        {
            return await Task.FromResult(Ok(_productService.ReadAll()));
        }
    }
}
