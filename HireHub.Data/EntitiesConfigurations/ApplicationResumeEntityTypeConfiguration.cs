using HireHub.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireHub.Data.EntitiesConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ApplicationResumeEntityTypeConfiguration:IEntityTypeConfiguration<ApplicationResume>
    {
        public void Configure(EntityTypeBuilder<ApplicationResume> builder)
        {
            builder.HasKey(t => new { t.ApplicationId, t.ResumeId });

            builder.HasOne(x => x.Application)
                .WithMany(t => t.ApplicationResumes)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
