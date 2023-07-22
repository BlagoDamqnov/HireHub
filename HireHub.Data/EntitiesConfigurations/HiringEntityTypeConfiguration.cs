using HireHub.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireHub.Data.EntitiesConfigurations
{
    public class HiringEntityTypeConfiguration : IEntityTypeConfiguration<HiringRecord>
    {
        public void Configure(EntityTypeBuilder<HiringRecord> builder)
        {
            builder.HasOne(hr => hr.Job)
       .WithMany()
       .HasForeignKey(hr => hr.JobId)
       .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(hr => hr.ApplicationUser)
                .WithMany()
                .HasForeignKey(hr => hr.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}