using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderSystemWebApi.DTO.Order;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepositoryService _orderService;
        private readonly IControllerServices _controllerService;
        private readonly IProblemService _problemService;
        public OrderController(IOrderRepositoryService orderRepositoryService, IControllerServices controllerServices, IProblemService problemService)
        {
            _orderService = orderRepositoryService;
            _controllerService = controllerServices;
            _problemService = problemService;
            
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Invalid product id will be ignored. Should have atleast one valid product id. Authorization of any role is required.
        /// </remarks>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDTO request)
        {
            try
            {
                return await TryCreateOrderAsync(request);
            }
            catch (ArgumentException e)
            {
                return BadRequest(_problemService.CreateBadRequestProblemDetails(e.Message, Request.Path));
            }
        }

        private async Task<IActionResult> TryCreateOrderAsync(OrderRequestDTO request)
        {
            var userId = await _controllerService.GetUserIdFromAuthorizationHeaderAsync(Request);

            await _orderService.CreateOrder(request, userId);
            
            return Ok("Order created.");
        }
    }
}
