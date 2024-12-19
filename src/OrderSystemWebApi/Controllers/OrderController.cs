using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderSystemWebApi.DTO.Order;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Mapper;

namespace OrderSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepositoryService _orderService;
        private readonly IControllerServices _controllerService;
        private readonly IProblemService _problemService;
        private readonly ILogger<OrderController> _logger;
        public OrderController
        (
            IOrderRepositoryService orderRepositoryService,
            IControllerServices controllerServices,
            IProblemService problemService,
            ILogger<OrderController> logger
        )
        {
            _orderService = orderRepositoryService;
            _controllerService = controllerServices;
            _problemService = problemService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new order for the authenticated user.
        /// </summary>
        /// <param name="request">The order request containing the order details.</param>
        /// <returns>
        /// A response indicating the result of the order creation process.
        /// </returns>
        /// <remarks>
        /// This endpoint allows an authenticated user to create a new order.
        /// If the request contains invalid arguments, a 400 Bad Request response is returned with the error details.
        /// If the order creation is successful, a 200 OK response is returned with a success message.
        /// Authorization is required.
        /// </remarks>
        /// <response code="200">If the order creation is successful.</response>
        /// <response code="400">If the request contains invalid arguments.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="500">If there is something not working on the server.</response>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDTO request)
        {
            try
            {
                return await ProcessOrderCreationAsync(request);
            }
            catch (ArgumentException e)
            {
                return await LogAndReturnBadRequestForOrderCreationAsync(e, request);
            }
            catch (Exception e)
            {
                return LogAndReturnInternalServerError(e);
            }
        }

        /// <summary>
        /// Retrieves all orders.
        /// </summary>
        /// <returns>
        /// A response containing a list of all orders.
        /// </returns>
        /// <remarks>
        /// This endpoint allows users with the Admin or Moderator role to retrieve all orders.
        /// If the request is successful, a 200 OK response is returned with the list of orders.
        /// Authorization with admin or moderator role is required.
        /// </remarks>
        /// <response code="200">If the request is successful and returns the list of orders.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403"> If the user is authenticated but does not have the admin or moderator role.</response>
        /// <response code="500">If there is something not working on the server.</response>
        [HttpGet("all-orders")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<IEnumerable<ReadOrderRequestDTO>>> GetAllOrders()
        {
            try
            {
                return await FetchAllOrdersAsync();
            }
            catch (Exception e)
            {
                return LogAndReturnInternalServerError(e);
            }
        }

        /// <summary>
        /// Retrieves an order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>
        /// A response containing the order details.
        /// </returns>
        /// <remarks>
        /// This endpoint allows users with the Admin or Moderator role to retrieve an order by its ID.
        /// If the order is not found, a 404 Not Found response is returned.
        /// If the request is successful, a 200 OK response is returned with the order details.
        /// </remarks>
        /// <response code="200">If the request is successful and returns the order details.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is authenticated but does not have the admin or moderator role.</response>
        /// <response code="404">If the order is not found.</response>
        /// <response code="500">If there is something not working on the server.</response>
        [HttpGet("order-by-id")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<ReadOrderRequestDTO>> GetOrderById([FromQuery] Guid id)
        {
            try
            {
                return await FetchOrderByIdAsync(id);
            }
            catch (Exception e)
            {
                return LogAndReturnInternalServerError(e);
            }
        }

        /// <summary>
        /// Retrieves all orders for a specific user by their user ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose orders to retrieve.</param>
        /// <returns>
        /// A response containing the list of orders for the specified user.
        /// </returns>
        /// <remarks>
        /// This endpoint allows users with the Admin or Moderator role to retrieve all orders for a specific user by their user ID.
        /// If the user ID is invalid, a 400 Bad Request response is returned.
        /// If the request is successful, a 200 OK response is returned with the list of orders.
        /// Authorization with admin or moderator role is required.
        /// </remarks>
        /// <response code="200">If the request is successful and returns the list of orders.</response>
        /// <response code="400">If the user ID is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is authenticated but does not have the admin or moderator role.</response>
        /// <response code="500">If there is something not working on the server.</response>
        [HttpGet("orders-by-user-id")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<IEnumerable<ReadOrderRequestDTO>>> GetUserOrders([FromQuery] Guid userId)
        {
            try
            {
                return await FetchUserOrdersAsync(userId);
            }
            catch (ArgumentException e)
            {
                return BadRequest(_problemService.CreateNotFoundProblemDetails(e.Message, Request.Path));
            }
        }

        /// <summary>
        /// Retrieves all orders for the authenticated user based on the token.
        /// </summary>
        /// <returns>
        /// A response containing the list of orders for the authenticated user.
        /// </returns>
        /// <remarks>
        /// This endpoint allows an authenticated user to retrieve all their orders based on the token.
        /// If the request is successful, a 200 OK response is returned with the list of orders.
        /// Authorization is required.
        /// </remarks>
        /// <response code="200">If the request is successful and returns the list of orders.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// /// <response code="500">If there is something not working on the server.</response>
        [HttpGet("orders-by-token")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReadOrderRequestDTO>>> GetUserOrdersByToken()
        {
            try
            {
                return await FetchUserOrderByTokenAsync();
            }
            catch (Exception e)
            {
                return LogAndReturnInternalServerError(e);
            }
        }

        private async Task<ActionResult<IEnumerable<ReadOrderRequestDTO>>> FetchUserOrderByTokenAsync()
        {
            var userId = await _controllerService.GetUserIdFromAuthorizationHeaderAsync(Request);

            var orders = await _orderService.GetAllUserOrdersAsync(userId);

            var filteredOrders = orders.Select(o => o.ToReadOrderDTO());

            _logger.LogInformation("[{RequestPath}] User {UserId} fetch his own orders.", Request.Path, userId);

            return Ok(filteredOrders);
        }

        private async Task<ActionResult<IEnumerable<ReadOrderRequestDTO>>> FetchUserOrdersAsync(Guid userId)
        {
            var userWhoRequest = _controllerService.GetUserIdFromAuthorizationHeaderAsync(Request);

            var userOrders = await _orderService.GetAllUserOrdersAsync(userId.ToString());

            var filteredOrders = userOrders.Select(o => o.ToReadOrderDTO());

            _logger.LogInformation("[{RequestPath}] User {UserId} get all orders of user {userId}", Request.Path, userWhoRequest, userId);

            return Ok(filteredOrders);
        }

        private async Task<IActionResult> ProcessOrderCreationAsync(OrderRequestDTO request)
        {
            var userId = await _controllerService.GetUserIdFromAuthorizationHeaderAsync(Request);

            await _orderService.CreateOrder(request, userId);

            _logger.LogInformation("[{RequestPath}] User {userId} is creating an order with details: {@details}", Request.Path, userId, request);

            return Ok("Order created.");
        }

        private async Task<ActionResult<IEnumerable<ReadOrderRequestDTO>>> FetchAllOrdersAsync()
        {
            var userId = _controllerService.GetUserIdFromAuthorizationHeaderAsync(Request);

            var orders = await _orderService.GetAllOrdersAsync();

            var FilteredOrders = orders.Select(o => o.ToReadOrderDTO());

            _logger.LogInformation("[{RequestPath}] User {UserId} get all orders record.", Request.Path, userId);

            return Ok(FilteredOrders);
        }

        private async Task<ActionResult<ReadOrderRequestDTO>> FetchOrderByIdAsync(Guid id)
        {
            var userId = _controllerService.GetUserIdFromAuthorizationHeaderAsync(Request);

            var order = await _orderService.GetOrderById(id);

            if (order == null)
                return NotFound(_problemService.CreateNotFoundProblemDetails($"Order with an id of {id} is not found.", Request.Path));

            _logger.LogInformation("[{RequestPath}] User {UserId} fetch an order by id with details {id}", Request.Path, userId, id);
            
            return Ok(order.ToReadOrderDTO());
        }

        private ObjectResult LogAndReturnInternalServerError(Exception e)
        {
            _logger.LogCritical("{@e}", e);

            return StatusCode(500, _problemService.CreateInternalServerErrorProblemDetails(e.Message, Request.Path));
        } 

        private async Task<BadRequestObjectResult> LogAndReturnBadRequestForOrderCreationAsync(ArgumentException e, OrderRequestDTO request)
        {
            var userid = await _controllerService.GetUserIdFromAuthorizationHeaderAsync(Request);

            _logger.LogInformation("[{RequestPath}] User {userId} failed to create an order with a message: '{message}' with request details: {@OrderDetails}", Request.Path, userid, e.Message, request);

            return BadRequest(_problemService.CreateBadRequestProblemDetails(e.Message, Request.Path));
        }
    }
}
