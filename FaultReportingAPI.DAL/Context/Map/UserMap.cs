using FaultReportingAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FaultReportingAPI.DAL.Context.Map
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.Email).IsUnique().HasFilter("[IsDeleted] = 0 AND [Email] IS NOT NULL");


            builder.Property(p => p.Name).HasMaxLength(100);

            builder.Property(p => p.Surname).HasMaxLength(100);

            builder.Property(p => p.Email).IsRequired().HasMaxLength(200);

            builder.Property(p => p.Password).HasMaxLength(200);

            builder.ToTable(nameof(User));
        }
    }
}
