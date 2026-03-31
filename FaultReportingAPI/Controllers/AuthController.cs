using FaultReportingAPI.BLL.Services.Abstract;
using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Functions;
using Microsoft.AspNetCore.Mvc;

namespace FaultReportingAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns></returns>
        /// <remarks>Authenticates a user using the provided email and password.</remarks>
        /// <response code="200">The status has been successfully updated.</response>
        /// <response code="400">Invalid parameters or bad request.</response>
        /// <response code="401">Unauthorized access (Token is missing or invalid).</response>
        /// <response code="404">No record found with the specified ID.</response>
        /// <response code="500">Internal server error (An unexpected error occurred on the server).</response>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseSingle<UserDto_Token>), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 404)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> LoginAsync([FromQuery] string email, [FromQuery] string password)
        {
            return GeneralFunctions.StatusCodeResult(this, await _userService.LoginUserAsync(email: email, password: password));
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="userDto">The user registration data.</param>
        /// <returns></returns>
        /// <remarks>Registers a new user.</remarks>
        /// <response code="201">The status has been successfully updated.</response>
        /// <response code="400">Invalid parameters or bad request.</response>
        /// <response code="401">Unauthorized access (Token is missing or invalid).</response>
        /// <response code="500">Internal server error (An unexpected error occurred on the server).</response>
        [ProducesResponseType(typeof(ResponseSingle<UserDto_Token>), 201)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 500)]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] UserDto_Request userDto)
        {
            return GeneralFunctions.StatusCodeResult(this, await _userService.RegisterAsync(userDto));
        }

    }
}
