using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderSystemWebApi.DTO.User;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Mapper;
using Serilog;

namespace OrderSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepositoryService _userService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IProblemService _problemService;
        private readonly ILogger<UserController> _logger;
        public UserController
        (
            IUserRepositoryService userService, 
            IJwtTokenService jwtTokenService, 
            IProblemService problemService,
            ILogger<UserController> logger
        )
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
            _problemService = problemService;
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
        /// Authorization not required.
        /// </remarks>
        /// <response code="200">Returns the user information and token.</response>
        /// <response code="401">If username or password is incorrect.</response>
        [HttpPost("log-in")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<LogInResponse>> LogIn([FromBody] LogInRequest request)
        {
            _logger.LogInformation("Test log");
            
            var user = await _userService.LogInUserAsync(request);

            if (user == null)
                return Unauthorized(_problemService.CreateUnauthorizeProblemDetails("Invalid username or password.", Request.Path));

            var token = await _jwtTokenService.GenerateToken(user);

            var response = user.ToLogInResponse(token);

            return Ok(response);
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// This endpoint used to register new account for a new user.
        /// If the username already exists, a 400 Bad Request response is returned.
        /// If the registration fails, a 500 Internal Server Error response is returned with the error details.
        /// If the registration is successful, a 200 OK response is returned with a success message.
        /// Authorization not required.
        /// </remarks>
        /// <response code="200">If the registration is successful.</response>
        /// <response code="400">If the username already exists.</response>
        /// <response code="500">If the registration fails.</response>
        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequestDTO request)
        {
            if (await _userService.IsUsernameAlreadyExists(request.Username))
                return BadRequest(_problemService.CreateBadRequestProblemDetails($"Username '{request.Username}' already exists.", Request.Path));

            var result = await _userService.RegisterUserAsync(request);

            if (result.Succeeded == false)
                return StatusCode(500, _problemService.CreateInternalServerErrorProblemDetails([.. result.Errors.Select(e => e.Description)], Request.Path));

            return Ok("User created succesfully.");
        }

        /// <summary>
        /// Registers a new moderator.
        /// </summary>
        /// <param name="request">The registration request containing the user details.</param>
        /// <returns>
        /// A response indicating the result of the registration process.
        /// </returns>
        /// <remarks>
        /// This endpoint allows for the registration of a new moderator. 
        /// If the username already exists, a 400 Bad Request response is returned.
        /// If the registration fails, a 500 Internal Server Error response is returned with the error details.
        /// If the registration is successful, a 200 OK response is returned with a success message.
        /// Only account with an admin role is authorized to access this endpoint.
        /// </remarks>
        /// <response code="200">If the registration is successful.</response>
        /// <response code="400">If the username already exists.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is authenticated but does not have the admin role.</response>
        /// <response code="500">If the registration fails.</response>
        [Authorize(Roles = "Admin")]
        [HttpPost("register-moderator")]
        public async Task<IActionResult> RegisterModerator([FromBody] RegisterUserRequestDTO request)
        {
            if (await _userService.IsUsernameAlreadyExists(request.Username))
                return BadRequest(_problemService.CreateBadRequestProblemDetails($"Username '{request.Username}' already exist.", Request.Path));

            var result = await _userService.RegisterModeratorAsync(request);

            if (result.Succeeded == false)
                return StatusCode(500, _problemService.CreateInternalServerErrorProblemDetails([.. result.Errors.Select(e => e.Description)], Request.Path));

            return Ok("Moderator created successfully.");
        }
    }
}
