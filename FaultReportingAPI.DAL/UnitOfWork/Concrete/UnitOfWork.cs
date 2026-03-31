using FaultReportingAPI.Core.Models.Base;
using FaultReportingAPI.DAL.Context;
using FaultReportingAPI.DAL.Repository.Abstract;
using FaultReportingAPI.DAL.Repository.Abstract.Base;
using FaultReportingAPI.DAL.Repository.Concrete;
using FaultReportingAPI.DAL.Repository.Concrete.Base;
using FaultReportingAPI.DAL.UnitOfWork.Abstract;

namespace FaultReportingAPI.DAL.UnitOfWork.Concrete
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        private IUserRepository _userRepository;
        public IUserRepository userRepository => _userRepository ??= new UserRepository(_context);

        private IFaultReportingRepository _faultReportingRepository;
        public IFaultReportingRepository faultReportingRepository => _faultReportingRepository ??= new FaultReportingRepository(_context);

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            return new GenericRepository<TEntity>(_context);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose() => _context.Dispose();
    }
}
