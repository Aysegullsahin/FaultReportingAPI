using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Models.Base;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FaultReportingAPI.Functions
{
    public static class GeneralFunctions
    {
        public static IActionResult StatusCodeResult<TResult>(ControllerBase controller, DataResult<TResult>? dataResult) where TResult : CoreBaseEntity
        {
            Response<TResult> response = new()
            {
                Data = dataResult?.Data,
                Message = dataResult?.Message,
                Count = dataResult?.Count,
                Success = dataResult.Success,
                ListData = dataResult?.ListData
            };
            return dataResult?.StatusCode switch
            {
                HttpStatusCode.OK => controller.Ok(response),
                HttpStatusCode.Created => controller.Created("", response),
                HttpStatusCode.NoContent => controller.NoContent(),
                HttpStatusCode.NotFound => controller.NotFound(response),
                HttpStatusCode.BadRequest => controller.BadRequest(response),
                HttpStatusCode.Conflict => controller.Conflict(response),
                HttpStatusCode.UnprocessableContent => controller.UnprocessableEntity(dataResult),
                HttpStatusCode.Forbidden => controller.Forbid(),
                HttpStatusCode.Unauthorized => controller.Unauthorized(),
                _ => controller.StatusCode((int)dataResult!.StatusCode!, dataResult.Message)
            };
        }

        public static IActionResult StatusCodeResult(ControllerBase controller, DataResult? dataResult)
        {
            Response response = new()
            {
                Message = dataResult?.Message,
                Success = dataResult?.Success,
            };
            return dataResult?.StatusCode switch
            {
                HttpStatusCode.OK => controller.Ok(response),
                HttpStatusCode.Created => controller.Created("", response),
                HttpStatusCode.NoContent => controller.NoContent(),
                HttpStatusCode.NotFound => controller.NotFound(response),
                HttpStatusCode.BadRequest => controller.BadRequest(response),
                HttpStatusCode.Conflict => controller.Conflict(response),
                HttpStatusCode.UnprocessableContent => controller.UnprocessableEntity(response),
                HttpStatusCode.Forbidden => controller.Forbid(),
                HttpStatusCode.Unauthorized => controller.Unauthorized(),
                _ => controller.StatusCode((int)dataResult!.StatusCode!, response.Message)
            };
        }

    }
}
