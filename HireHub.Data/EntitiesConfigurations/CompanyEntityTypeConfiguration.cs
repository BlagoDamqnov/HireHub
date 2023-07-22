using HireHub.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireHub.Data.EntitiesConfigurations
{
    public class CompanyEntityTypeConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasIndex(c => c.Name)
                .IsUnique();
            builder.HasIndex(c => c.ContactEmail)
                .IsUnique();
            builder.HasIndex(c => c.ContactPhone)
                .IsUnique();

            builder.HasMany(c => c.Jobs)
                .WithOne(j => j.Company)
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}