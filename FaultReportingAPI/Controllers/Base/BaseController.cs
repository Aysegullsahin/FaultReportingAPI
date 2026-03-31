using FaultReportingAPI.BLL.Services.Abstract.Base;
using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Enums;
using FaultReportingAPI.Core.Models.Base;
using FaultReportingAPI.Functions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FaultReportingAPI.Controllers.Base
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class BaseController<TEntity>(IBaseService<TEntity> service) : ControllerBase where TEntity : BaseEntity
    {
        private readonly IBaseService<TEntity> _service = service;

 
        [HttpDelete]
        public virtual async Task<IActionResult> DeleteAsync([FromQuery] long id)
        {
            return GeneralFunctions.StatusCodeResult(this, await _service.DeleteAsync(id: id));
        }
  
        [HttpGet]
        public virtual async Task<IActionResult> GetAsync(long id)
        {
            return GeneralFunctions.StatusCodeResult(this, await _service.GetAsync(id));
        }
    }
}
