using FaultReportingAPI.Core.Models.Base;
using FaultReportingAPI.DAL.Repository.Abstract;
using FaultReportingAPI.DAL.Repository.Abstract.Base;

namespace FaultReportingAPI.DAL.UnitOfWork.Abstract
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository { get; }
        IFaultReportingRepository faultReportingRepository { get; }
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        Task CompleteAsync();

    }
}
