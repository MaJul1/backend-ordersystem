using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderSystemWebApi.DTO.User;
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

        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A response containing user information and JWT token if authentication is successfull.</returns>
        /// <remarks>
        /// This endpoint allows a user to log in by providing their username and password. 
        /// If the credentials are correct, a JWT token is generated and returned along with the user information.
        /// Token validity is 30 minutes upon token creation.
        /// </remarks>
        /// <response code="401">If username or password is incorrect.</response>
        /// <response code="200">Returns the user information and token.</response>
        [HttpPost("log-in")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<LogInResponse>> LogIn([FromBody] LogInRequest request)
        {
            var user = await _userService.LogInUserAsync(request);
            
            if (user == null)
                return Unauthorized();
            
            var token = await _jwtTokenService.GenerateToken(user);

            var response = user.ToLogInResponse(token);
            return Ok(response);
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequestDTO request)
        {
            var result = await _userService.RegisterUserAsync(request);

            if (result.Succeeded == false)
                return StatusCode(500, result.Errors.Select(e => e.Description));

            return Ok("User created succesfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register-moderator")]
        public async Task<IActionResult> RegisterModerator([FromBody] RegisterUserRequestDTO request)
        {
            var result = await _userService.RegisterModeratorAsync(request);

            if (result.Succeeded == false)
                return StatusCode(500, result.Errors.Select(e => e.Description));

            return Ok("Moderator created successfully.");
        }
    }
}
