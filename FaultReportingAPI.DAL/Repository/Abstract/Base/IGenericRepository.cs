using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Enums;
using FaultReportingAPI.Core.Models.Base;
using System.Linq.Expressions;

namespace FaultReportingAPI.DAL.Repository.Abstract.Base
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<DataResult<TEntity>> GetByIdAsync(long id);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);
        Task<DataResult<TEntity>> CrudAsync(TEntity entity, EntityStateEnum entityState, Dictionary<string, object>? changedFields = null, long? userId = null);

        Task<DataResult<CoreBaseEntity>> GetAsync<TResult>(Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, TResult>>? selector = null) where TResult : CoreBaseEntity;

        Task<DataResult<CoreBaseEntity>> GetListAsync<TResult>(Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, TResult>>? selector = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int pageNumber = 1, int pageSize = 10) where TResult : CoreBaseEntity;
    }
}
