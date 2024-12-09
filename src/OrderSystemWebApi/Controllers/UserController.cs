using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderSystemWebApi.DTO;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Mapper;

namespace OrderSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepositoryService _userService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger _logger;
        public UserController (IUserRepositoryService userService, IJwtTokenService jwtTokenService, ILogger<UserController> logger)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
            
        }

        [HttpPost("log-in")]
        public async Task<ActionResult<LogInResponse>> LogIn([FromBody] LogInRequest request)
        {
            var user = await _userService.LogInUserAsync(request);
            
            if (user == null)
                return NotFound("Username or Password is incorrect.");
            
            var token = await _jwtTokenService.GenerateToken(user);

            var response = user.ToLogInResponse(token);
            return response;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] WriteUserRequestDTO request)
        {
            var result = await _userService.RegisterUserAsync(request);

            if (result.Succeeded == false)
            {
                _logger.LogCritical("Unable to register user. {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                return StatusCode(500);
            }

            return Ok("User created successfully");
        }
    }
}
