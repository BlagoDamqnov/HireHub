using HireHub.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireHub.Data.EntitiesConfigurations
{
    public class ApplicationEntityTypeConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.HasOne(a => a.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(a => a.Resume)
                .WithMany(r => r.Applications)
                .HasForeignKey(a => a.ResumeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}