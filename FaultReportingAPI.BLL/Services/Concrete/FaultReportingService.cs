using FaultReportingAPI.BLL.Helpers;
using FaultReportingAPI.BLL.Mapper.Abstract;
using FaultReportingAPI.BLL.Services.Abstract;
using FaultReportingAPI.BLL.Services.Concrete.Base;
using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Enums;
using FaultReportingAPI.Core.Models;
using FaultReportingAPI.Core.Models.Base;
using FaultReportingAPI.DAL.UnitOfWork.Abstract;
using LinqKit;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace FaultReportingAPI.BLL.Services.Concrete
{
    public class FaultReportingService(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor, IFaultReportingMapper mapper) : BaseService<FaultReporting>(uow, httpContextAccessor), IFaultReportingService
    {
        private readonly IFaultReportingMapper _mapper = mapper;

        #region Crud Operations 
        public async Task<DataResult<CoreBaseEntity>> ChangeStatusAsync(long id, StatusEnum newStatus)
        {
            var dataResult = await _uow.faultReportingRepository.GetByIdAsync(id);

            if (!dataResult.Success || dataResult.Data == null)
                return new()
                {
                    Success = dataResult.Success,
                    Message = dataResult.Message,
                    StatusCode = dataResult.StatusCode
                };
            if (dataResult.Data.Status == newStatus)
            {
                return new DataResult<CoreBaseEntity>
                {
                    Success = false,
                    Message = "Status is already set to the specified value.",
                    StatusCode = HttpStatusCode.UnprocessableContent,
                };
            }
            if (StatusHelper.IsTerminal(dataResult.Data.Status))
            {
                return new()
                {
                    Success = false,
                    Message = "This record cannot be updated because it is in a terminal state.",
                    StatusCode = HttpStatusCode.UnprocessableContent
                };
            }
            if (!StatusHelper.CanTransition(dataResult.Data.Status, newStatus))
            {
                return new()
                {
                    Success = false,
                    Message = $"Invalid status transition: {dataResult.Data.Status} cannot be changed to {newStatus}.",
                    StatusCode = HttpStatusCode.UnprocessableContent
                };
            }
            dataResult.Data.Status = newStatus;
            var result = await _uow.faultReportingRepository.CrudAsync(entity: dataResult.Data, entityState: EntityStateEnum.Update, changedFields: new Dictionary<string, object> { ["Status"] = newStatus }, userId: userId);
             
            if (!result.Success || result.Data == null)
            {
                return new DataResult<CoreBaseEntity>
                {
                    Success = false,
                    Message = result.Message,
                    StatusCode = result.StatusCode
                };
            }

            return new DataResult<CoreBaseEntity>
            {
                Success = true,
                Data = _mapper.ToDto_S(result.Data),
                StatusCode = result.StatusCode
            };
        }
        public async Task<DataResult<CoreBaseEntity>> AddAsync(FaultReportingDto_AddRequest entityDto)
        {
            string location = entityDto.Location.Trim().ToLower();
            var oneHourAgo = DateTime.Now.AddHours(-1);

            if (await _uow.faultReportingRepository.AnyAsync(x => x.Location!.Trim().ToLower() == location &&
                x.CreatedDate >= oneHourAgo))
            {
                return new DataResult<CoreBaseEntity>
                {
                    Success = false,
                    Message = "A second fault report cannot be generated for the same location within 1 hour.",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            var entity = _mapper.ToAddEntity(entityDto);
            entity.Status = StatusEnum.New;

            var dataResult = await _uow.faultReportingRepository.CrudAsync(entity: entity, entityState: EntityStateEnum.Insert, userId: userId);

            if (!dataResult.Success || dataResult.Data == null)
            {
                return new DataResult<CoreBaseEntity>
                {
                    Success = false,
                    Message = dataResult.Message,
                    StatusCode = dataResult.StatusCode
                };
            }

            return new DataResult<CoreBaseEntity>
            {
                Success = true,
                Data = _mapper.ToDto_S(dataResult.Data),
                StatusCode = dataResult.StatusCode
            };
        }
        public async Task<DataResult<CoreBaseEntity>> UpdateAsync(FaultReportingDto_UpdateRequest entityDto, Dictionary<string, object>? changedFields = null)
        {
            var entity = _mapper.ToUpdateEntity(entityDto);
            changedFields = new()
            {
                ["Title"] = entityDto.Title,
                ["Description"] = entityDto.Description,
                ["Priority"] = entityDto.Priority,
                ["Location"] = entityDto.Location
            };
            var dataResult = await _uow.faultReportingRepository.CrudAsync(entity: entity, entityState: EntityStateEnum.Update, changedFields: changedFields,userId: userId);

            if (!dataResult.Success || dataResult.Data == null)
            {
                return new DataResult<CoreBaseEntity>
                {
                    Success = false,
                    Message = dataResult.Message,
                    StatusCode = dataResult.StatusCode
                };
            }

            return new DataResult<CoreBaseEntity>
            {
                Success = true,
                Data = _mapper.ToDto_S(dataResult.Data),
                StatusCode = dataResult.StatusCode
            };
        }
        #endregion

        #region List
        public async Task<DataResult<CoreBaseEntity>> GetFaultReportingListAsync(StatusEnum? status, PriorityLevelEnum? priorityLevel, string? location, FaultReportSortByEnum? sortBy, bool isDescending, int pageNumber = 1, int pageSize = 10)
        {
            Func <IQueryable<FaultReporting>, IOrderedQueryable<FaultReporting>> orderBy = q => q.OrderBy(x => x.CreatedDate);
            var filter = PredicateBuilder.New<FaultReporting>(x => !x.IsDeleted);

            if (role == RoleEnum.User)
                filter = filter.And(x => x.CreatedById == userId);
            
            if (status != null && status != 0)
                filter = filter.And(x => x.Status == status);

            if (priorityLevel != null && priorityLevel != 0)
                filter = filter.And(x => x.Priority == priorityLevel);

            if (!string.IsNullOrEmpty(location))
                filter = filter.And(x => x.Location!.Contains(location));

            if (sortBy != null && sortBy != 0)
            {
                orderBy = sortBy switch
                {
                    FaultReportSortByEnum.PriorityLevel => isDescending ? (q => q.OrderByDescending(x => x.Priority)) : (q => q.OrderBy(x => x.Priority)),
                    FaultReportSortByEnum.CreatedDate => isDescending ? (q => q.OrderByDescending(x => x.CreatedDate)) : (q => q.OrderBy(x => x.CreatedDate)),
                    _ => q => q.OrderBy(x => x.CreatedDate)
                };
            }
            return await _uow.faultReportingRepository.GetListAsync<FaultReportingDto_L>(filter: filter,orderBy: orderBy, pageNumber: pageNumber, pageSize: pageSize) ;
        }
        #endregion

        #region Single
        public async override Task<DataResult<CoreBaseEntity>> GetAsync(long id)
        {
            return await _uow.faultReportingRepository.GetAsync<FaultReportingDto_S>(filter: x => x.CreatedById == userId && x.Id == id && !x.IsDeleted);
        }
        #endregion
    }
}
