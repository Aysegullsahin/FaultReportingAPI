using FaultReportingAPI.BLL.Helpers;
using FaultReportingAPI.BLL.Services.Abstract.Base;
using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Enums;
using FaultReportingAPI.Core.Models.Base;
using FaultReportingAPI.DAL.Repository.Abstract.Base;
using FaultReportingAPI.DAL.UnitOfWork.Abstract;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Security.Claims;

namespace FaultReportingAPI.BLL.Services.Concrete.Base
{
    public class BaseService<TEntity>(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor) : IBaseService<TEntity> where TEntity : BaseEntity
    {
        protected readonly IUnitOfWork _uow = uow;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        protected IGenericRepository<TEntity> repository => _uow.Repository<TEntity>();
        private ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;
        protected long userId => User.GetUserId();
        protected RoleEnum role => User.GetRole();

        #region Crud Operations
        public async Task<DataResult> DeleteAsync(long id)
        {
            var entity = await _uow.Repository<TEntity>().GetByIdAsync(id: id);
            if (entity.Data == null)
            {
                return new DataResult()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Success = false,
                    Message = "Record not found."
                };
            }
            var response = await CrudAsync(userId: userId, entity: entity.Data, entityState: EntityStateEnum.Delete);
            return new DataResult()
            {
                StatusCode = response.StatusCode,
                Success = response.Success,
                Message = response.Message
            };
        }

        private async Task<DataResult<TEntity>> CrudAsync(long userId, TEntity entity, EntityStateEnum entityState)
        {
            return await repository.CrudAsync(entity: entity, entityState: entityState, userId: userId);
        }
        #endregion

        public virtual async Task<DataResult<CoreBaseEntity>> GetAsync(long id)
        {
           return await repository.GetAsync<TEntity>();
        }


        public virtual async Task<DataResult<CoreBaseEntity>> GetListAsync<TResult>(Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, TResult>>? selector = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int pageNumber = 1, int pageSize = 10) where TResult : CoreBaseEntity
        {
           return await repository.GetListAsync(filter: filter, selector: selector, orderBy: orderBy, pageNumber: pageNumber, pageSize: pageSize);
        }

    }
}
