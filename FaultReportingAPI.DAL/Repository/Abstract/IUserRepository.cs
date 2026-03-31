using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Models;
using FaultReportingAPI.Core.Models.Base;
using FaultReportingAPI.DAL.Repository.Abstract.Base;
using System.Linq.Expressions;

namespace FaultReportingAPI.DAL.Repository.Abstract
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<DataResult<CoreBaseEntity>> Login<TResult>(Expression<Func<User, bool>>? filter = null, Expression<Func<User, TResult>>? selector = null) where TResult : CoreBaseEntity;

    }
}
