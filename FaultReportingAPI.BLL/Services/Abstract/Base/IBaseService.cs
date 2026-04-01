using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Models.Base;
using System.Linq.Expressions;

namespace FaultReportingAPI.BLL.Services.Abstract.Base
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity
    {
        Task<DataResult> DeleteAsync(long id);
        Task<DataResult<CoreBaseEntity>> GetAsync(long id);
        Task<DataResult<CoreBaseEntity>> GetListAsync<TResult>(Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, TResult>>? selector = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int pageNumber = 1,int pageSize = 10) where TResult : CoreBaseEntity;
    }
}
