using FaultReportingAPI.BLL.Services.Abstract;
using FaultReportingAPI.Controllers.Base;
using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Enums;
using FaultReportingAPI.Core.Models;
using FaultReportingAPI.Functions;
using Microsoft.AspNetCore.Mvc;

namespace FaultReportingAPI.Controllers
{
    public class FaultReportingController(IFaultReportingService faultReportingService) : BaseController<FaultReporting>(faultReportingService)
    {
        private readonly IFaultReportingService _faultReportingService = faultReportingService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">The unique identifier of the fault reporting record whose status will be updated.</param>
        /// <param name="newStatus">The new status value to be assigned to the record.</param>
        /// <returns>Returns the result of the operation along with the updated record information.</returns>
        /// <remarks>Updates the status of the specified fault reporting record with the provided new status value. This operation can only be performed by authorized users and is applied based on the given record identifier.</remarks>
        /// <response code="201">The status has been successfully updated.</response>
        /// <response code="400">Invalid parameters or bad request.</response>
        /// <response code="401">Unauthorized access (Token is missing or invalid).</response>
        /// <response code="403">Forbidden (Admin role is required).</response>
        /// <response code="404">No record found with the specified ID.</response>
        /// <response code="422">Unprocessable entity (Validation errors or business logic violation).</response>
        /// <response code="500">Internal server error (An unexpected error occurred on the server).</response>
        [HttpPut]
        [TokenAuthorize(RoleEnum.Admin)]
        [ProducesResponseType(typeof(ResponseSingle<FaultReportingDto_S>), 201)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 404)]
        [ProducesResponseType(typeof(Response), 422)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> ChangeStatusAsync([FromQuery] long id, [FromQuery] StatusEnum newStatus)
        {
            var data = GeneralFunctions.StatusCodeResult(this, await _faultReportingService.ChangeStatusAsync(id: id, newStatus));
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// This endpoint updates the details of a fault reporting record identified within the request body.
        /// The operation can only be performed by authorized users.
        /// </remarks>
        /// <param name="entity">The request object containing the updated values of the fault reporting record.</param>
        /// <returns></returns>
        /// <response code="201">The status has been successfully updated.</response>
        /// <response code="400">Invalid parameters or bad request.</response>
        /// <response code="401">Unauthorized access (Token is missing or invalid).</response>
        /// <response code="404">No record found with the specified ID.</response>
        /// <response code="500">Internal server error (An unexpected error occurred on the server).</response>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseSingle<FaultReportingDto_S>), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 404)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> UpdateAsync([FromBody] FaultReportingDto_UpdateRequest entity)
        {
            return GeneralFunctions.StatusCodeResult(this, await _faultReportingService.UpdateAsync(entityDto: entity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// This endpoint supports filtering by status, priority level, and location, as well as sorting and pagination.
        /// The results can be ordered using the specified sorting field and direction.
        /// </remarks>
        /// <param name="status">Filters records by their current status.</param>
        /// <param name="priorityLevel">Filters records by priority level.</param>
        /// <param name="location">Optional. Filters records by location (partial match).</param>
        /// <param name="sortBy">Specifies the field used for sorting the results.</param>
        /// <param name="isDescending">Determines whether the sorting should be in descending order.</param>
        /// <param name="pageNumber">Specifies the page number for pagination (starts from 1).</param>
        /// <param name="pageSize">Specifies the number of records to return per page.</param>
        /// <returns></returns>      
        /// <response code="200">The status has been successfully updated.</response>
        /// <response code="400">Invalid parameters or bad request.</response>
        /// <response code="401">Unauthorized access (Token is missing or invalid).</response>
        /// <response code="404">No record found with the specified ID.</response>
        /// <response code="500">Internal server error (An unexpected error occurred on the server).</response>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseList<FaultReportingDto_L>), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 404)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> GetAllAsync([FromQuery] StatusEnum status, [FromQuery] PriorityLevelEnum priorityLevel, [FromQuery] FaultReportSortByEnum sortBy, [FromQuery] string? location = null, [FromQuery] bool isDescending = false, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return GeneralFunctions.StatusCodeResult(this, await _faultReportingService.GetFaultReportingListAsync(status: status, priorityLevel: priorityLevel, location: location, sortBy, isDescending: isDescending, pageNumber: pageNumber, pageSize: pageSize));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <remarks>
        /// Use this endpoint to submit a new fault reporting to the system. 
        /// Ensure all required fields in the request body are provided.
        /// </remarks>
        /// <response code="201">The status has been successfully updated.</response>
        /// <response code="400">Invalid parameters or bad request.</response>
        /// <response code="401">Unauthorized access (Token is missing or invalid).</response>
        /// <response code="500">Internal server error (An unexpected error occurred on the server).</response>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseSingle<FaultReportingDto_S>), 201)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> AddAsync([FromBody] FaultReportingDto_AddRequest entity)
        {
            return GeneralFunctions.StatusCodeResult(this, await _faultReportingService.AddAsync(entityDto: entity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">The unique identifier of the record.</param>
        /// <returns>       
        /// </returns>
        /// <remarks>Retrieves a specific record by its unique identifier.</remarks>
        /// <response code="200">The status has been successfully updated.</response>
        /// <response code="400">Invalid parameters or bad request.</response>
        /// <response code="401">Unauthorized access (Token is missing or invalid).</response>
        /// <response code="404">No record found with the specified ID.</response>
        /// <response code="500">Internal server error (An unexpected error occurred on the server).</response>
        [ProducesResponseType(typeof(ResponseSingle<FaultReportingDto_S>), 200)]
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
        /// Deletes the specified fault report.
        /// </remarks>
        /// <response code="201">The status has been successfully updated.</response>
        /// <response code="400">Invalid parameters or bad request.</response>
        /// <response code="401">Unauthorized access (Token is missing or invalid).</response>
        /// <response code="404">No record found with the specified ID.</response>
        /// <response code="500">Internal server error (An unexpected error occurred on the server).</response>
        [ProducesResponseType(typeof(DataResult), 201)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 404)]
        [ProducesResponseType(typeof(Response), 500)]
        public override Task<IActionResult> DeleteAsync(long id)
        {
            return base.DeleteAsync(id);
        }
    }
}
