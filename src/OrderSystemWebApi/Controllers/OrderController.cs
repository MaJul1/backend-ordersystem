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
        public OrderController(IOrderRepositoryService orderRepositoryService, IControllerServices controllerServices)
        {
            _orderService = orderRepositoryService;
            _controllerService = controllerServices;
            
        }

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
                return BadRequest(GetArgumentExceptionProblemDetails(e));
            }
        }

        private async Task<IActionResult> TryCreateOrderAsync(OrderRequestDTO request)
        {
            var userId = await _controllerService.GetUserIdFromAuthorizationHeaderAsync(Request);

            await _orderService.CreateOrder(request, userId);
            
            return Ok("Order created.");
        }

        //TODO: create service for this.
        private ProblemDetails GetArgumentExceptionProblemDetails(ArgumentException error)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Argument Exception",
                Detail = error.Message ?? "An argument was invalid.",
                Instance = HttpContext.Request.Path
            };

            return problemDetails;
        }
    }
}
