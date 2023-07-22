using HireHub.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireHub.Data.EntitiesConfigurations
{
    public class JobEntityTypeConfiguration : IEntityTypeConfiguration<Job>

    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.Property(j => j.MinSalary)
                .HasColumnType("money");

            builder.Property(j => j.MaxSalary)
                .HasColumnType("money");
        }
    }
}