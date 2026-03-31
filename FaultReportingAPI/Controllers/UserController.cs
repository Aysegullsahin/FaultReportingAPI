using FaultReportingAPI.BLL.Services.Abstract;
using FaultReportingAPI.BLL.Services.Concrete;
using FaultReportingAPI.Controllers.Base;
using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Enums;
using FaultReportingAPI.Core.Models;
using FaultReportingAPI.Functions;
using Microsoft.AspNetCore.Mvc;

namespace FaultReportingAPI.Controllers
{
    public class UserController(IUserService userService) : BaseController<User> (service: userService)
    {
        private readonly IUserService _userService = userService;

        /// <summary>
        ///        
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve. Default value is 1.</param>
        /// <param name="pageSize">The number of records per page. Default value is 10.</param>
        /// <returns></returns>
        /// <remarks>
        /// Retrieves a paginated list of users. This endpoint is accessible only by users with the Admin role.
        /// </remarks>
        /// <response code="200">The status has been successfully updated.</response>
        /// <response code="400">Invalid parameters or bad request.</response>
        /// <response code="401">Unauthorized access (Token is missing or invalid).</response>
        /// <response code="403">Forbidden (Admin role is required).</response>
        /// <response code="404">No record found with the specified ID.</response>
        /// <response code="500">Internal server error (An unexpected error occurred on the server).</response>
        [ProducesResponseType(typeof(ResponseList<UserDto_L>), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 404)]
        [ProducesResponseType(typeof(Response), 500)]
        [HttpGet]
        [TokenAuthorize(RoleEnum.Admin)]
        public async Task<IActionResult> GetAllAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize=10)
        {
            return GeneralFunctions.StatusCodeResult(this, await _userService.GetListAsync<UserDto_L>(pageNumber: pageNumber, pageSize: pageSize));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">The unique identifier of the record.</param>
        /// <returns>
        /// Returns the requested record.
        /// </returns>
        /// <remarks> Retrieves a specific record by its unique identifier.</remarks>
        /// <response code="200">The status has been successfully updated.</response>
        /// <response code="400">Invalid parameters or bad request.</response>
        /// <response code="401">Unauthorized access (Token is missing or invalid).</response>
        /// <response code="404">No record found with the specified ID.</response>
        /// <response code="500">Internal server error (An unexpected error occurred on the server).</response>
        [ProducesResponseType(typeof(ResponseSingle<UserDto_S>), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 404)]
        [ProducesResponseType(typeof(Response), 500)]
        public override Task<IActionResult> GetAsync(long id)
        {
            return base.GetAsync(id);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="id">The unique identifier of the record to delete.</param>
        /// <returns></returns>
        /// <remarks>
        /// This operation permanently deletes the user account associated with the provided unique identifier (ID).
        /// </remarks>
        /// <response code="201">The status has been successfully updated.</response>
        /// <response code="400">Invalid parameters or bad request.</response>
        /// <response code="401">Unauthorized access (Token is missing or invalid).</response>
        /// <response code="404">No record found with the specified ID.</response>
        /// <response code="500">Internal server error (An unexpected error occurred on the server).</response>
        [ProducesResponseType(typeof(DataResult), 201)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 404)]
        [ProducesResponseType(typeof(Response), 500)] public override Task<IActionResult> DeleteAsync([FromQuery] long id)
        {
            return base.DeleteAsync(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// This endpoint updates the details of a user record identified within the request body.
        /// The operation can only be performed by authorized users.
        /// </remarks>
        /// <param name="entity">The request object containing the updated values of the fault user record.</param>
        /// <returns></returns>
        /// <response code="201">The status has been successfully updated.</response>
        /// <response code="400">Invalid parameters or bad request.</response>
        /// <response code="401">Unauthorized access (Token is missing or invalid).</response>
        /// <response code="404">No record found with the specified ID.</response>
        /// <response code="500">Internal server error (An unexpected error occurred on the server).</response>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseSingle<UserDto_S>), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 404)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> UpdateAsync([FromBody] UserDto_Request entity)
        {
            return GeneralFunctions.StatusCodeResult(this, await _userService.UpdateAsync(entityDto: entity));
        }
    }
}
