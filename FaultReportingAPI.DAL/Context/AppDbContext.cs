using FaultReportingAPI.Core.Models;
using FaultReportingAPI.DAL.Context.Map;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace FaultReportingAPI.DAL.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        //private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public DbSet<User> Users { get; set; }
        public DbSet<FaultReporting> FaultReporting { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap ());
            modelBuilder.ApplyConfiguration(new FaultReportingMap ());
        }
        //public long? CurrentUserId
        //{
        //    get
        //    {
        //        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

        //        return long.TryParse(userIdClaim, out var id) ? id : null;
        //    }
        //}
    }

    public class DesignTimeDbContextFactory
       : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(
            "Server=DESKTOP-2CCS1EP\\SQLEXPRESS;Database=FaultReportingDb;Trusted_Connection=True;TrustServerCertificate=True;"
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
