using FaultReportingAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FaultReportingAPI.DAL.Context.Map
{
    public class FaultReportingMap : IEntityTypeConfiguration<FaultReporting>
    {
        public void Configure(EntityTypeBuilder<FaultReporting> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title).HasMaxLength(100);
            builder.Property(p => p.Description).HasMaxLength(2000);
            builder.Property(p => p.Location).HasMaxLength(500);

            builder.ToTable(nameof(FaultReporting));
        }
    }
}
