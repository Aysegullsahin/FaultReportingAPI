using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Models;
using FaultReportingAPI.Core.Models.Base;
using FaultReportingAPI.DAL.Context;
using FaultReportingAPI.DAL.Repository.Abstract;
using FaultReportingAPI.DAL.Repository.Concrete.Base;
using System.Linq.Expressions;

namespace FaultReportingAPI.DAL.Repository.Concrete
{
    public class FaultReportingRepository(AppDbContext context) : GenericRepository<FaultReporting>(context), IFaultReportingRepository
    {
        public override Task<DataResult<CoreBaseEntity>> GetListAsync<TResult>(Expression<Func<FaultReporting, bool>>? filter = null, Expression<Func<FaultReporting, TResult>>? selector = null, Func<IQueryable<FaultReporting>, IOrderedQueryable<FaultReporting>>? orderBy = null, int pageNumber = 1, int pageSize = 10)
        {
            if (selector != null)
                return base.GetListAsync(filter, selector, orderBy, pageNumber, pageSize);

            return base.GetListAsync(filter, selector : x=>  new FaultReportingDto_L
            {
                Id = x.Id,
                Description = x.Description,
                CreatedDate = x.CreatedDate,
                CreatedUserName = x.CreatedUser.Name,
                CreatedUserSurname = x.CreatedUser.Surname,
                Location = x.Location,
                Priority = x.Priority,
                Status = x.Status,
                Title = x.Title,    
                UpdatedDate = x.UpdatedDate,
                UpdatedUserName = x.UpdatedUser.Name,
                UpdatetedUserSurname = x.UpdatedUser.Surname,
            }, orderBy, pageNumber, pageSize);
        }
        public override  Task<DataResult<CoreBaseEntity>> GetAsync<TResult>(Expression<Func<FaultReporting, bool>>? filter = null, Expression<Func<FaultReporting, TResult>>? selector = null)
        {
            if (selector != null)
               return BaseGetByAsync(filter, selector);

            return BaseGetByAsync(filter, selector : x=> new FaultReportingDto_S
            {
                Id = x.Id,
                Description = x.Description,
                CreatedDate = x.CreatedDate,
                CreatedUserName = x.CreatedUser.Name,
                CreatedUserSurname = x.CreatedUser.Surname,
                Location = x.Location,
                Priority = x.Priority,
                Status = x.Status,
                Title = x.Title,
                UpdatedDate = x.UpdatedDate,
                UpdatedUserName = x.UpdatedUser.Name,
                UpdatetedUserSurname = x.UpdatedUser.Surname,
            });
        }
    }
}
