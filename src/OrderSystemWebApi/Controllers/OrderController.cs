using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderSystemWebApi.DTO.Order;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Mapper;
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
        public OrderController
        (
            IOrderRepositoryService orderRepositoryService, 
            IControllerServices controllerServices, 
            IProblemService problemService
        )
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
        /// Invalid product id will be ignored. Should have atleast one valid product id. Authorization is required.
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

        [HttpGet("all-orders")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<IEnumerable<ReadOrderRequestDTO>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            var FilteredOrders = orders.Select(o => o.ToReadOrderDTO());

            return Ok(FilteredOrders);
        }

        [HttpGet("order-by-id")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<ReadOrderRequestDTO>> GetOrderById([FromQuery] Guid id)
        {
            var order = await _orderService.GetOrderById(id);

            if (order == null)
                return NotFound(_problemService.CreateNotFoundProblemDetails($"Order with an id of {id} is not found.", Request.Path));

            return Ok(order.ToReadOrderDTO());
        }

        [HttpGet("orders-by-user-id")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<IEnumerable<ReadOrderRequestDTO>>> GetUserOrders([FromQuery] Guid userId)
        {
            try
            {
                return await TryGetUserOrders(userId);
            }
            catch (ArgumentException e)
            {
                return BadRequest(_problemService.CreateNotFoundProblemDetails(e.Message, Request.Path));
            }
        }

        [HttpGet("orders-by-token")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReadOrderRequestDTO>>> GetUserOrdersByToken()
        {
            var userId = await _controllerService.GetUserIdFromAuthorizationHeaderAsync(Request);
            
            var orders = await _orderService.GetAllUserOrdersAsync(userId);

            var filteredOrders = orders.Select(o => o.ToReadOrderDTO());

            return Ok(filteredOrders);
        }

        private async Task<ActionResult<IEnumerable<ReadOrderRequestDTO>>> TryGetUserOrders(Guid userId)
        {
            var userOrders = await _orderService.GetAllUserOrdersAsync(userId.ToString());

            var filteredOrders = userOrders.Select(o => o.ToReadOrderDTO());

            return Ok(filteredOrders);
        }

        private async Task<IActionResult> TryCreateOrderAsync(OrderRequestDTO request)
        {
            var userId = await _controllerService.GetUserIdFromAuthorizationHeaderAsync(Request);

            await _orderService.CreateOrder(request, userId);
            
            return Ok("Order created.");
        }
    }
}
