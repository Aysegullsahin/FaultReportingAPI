using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Models;
using FaultReportingAPI.Core.Models.Base;
using FaultReportingAPI.DAL.Context;
using FaultReportingAPI.DAL.Repository.Abstract;
using FaultReportingAPI.DAL.Repository.Concrete.Base;
using System.Linq.Expressions;

namespace FaultReportingAPI.DAL.Repository.Concrete
{
    public class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository
    {
        public async Task<DataResult<CoreBaseEntity>> Login<TResult>(Expression<Func<User, bool>>? filter = null, Expression<Func<User, TResult>>? selector = null) where TResult : CoreBaseEntity
        {
            if (selector != null)
                return await BaseGetByAsync(filter, selector);

            return await BaseGetByAsync(filter, selector: x => new UserDto_LoginResponse()
            {
                Id = x.Id,
                Role = x.Role,
                HashPassword = x.Password ?? "",
                Email = x.Email,
            });
        }
        public override Task<DataResult<CoreBaseEntity>> GetAsync<TResult>(Expression<Func<User, bool>>? filter = null, Expression<Func<User, TResult>>? selector = null)
        {
            if (selector != null)
                return BaseGetByAsync(filter, selector);

            return BaseGetByAsync(filter, selector: x => new UserDto_S
            {
                Email = x.Email,       
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
            });
        }
        public override Task<DataResult<CoreBaseEntity>> GetListAsync<TResult>(Expression<Func<User, bool>>? filter = null, Expression<Func<User, TResult>>? selector = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, int pageNumber = 1, int pageSize = 10)
        {
            if (selector != null)
                return base.GetListAsync(filter, selector, orderBy, pageNumber, pageSize);

            return base.GetListAsync(filter, selector: x=> new UserDto_L
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                Surname = x.Surname,
            },  orderBy , pageNumber, pageSize);
        }
    }
}
