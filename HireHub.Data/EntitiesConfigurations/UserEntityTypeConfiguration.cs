using System.Diagnostics.CodeAnalysis;
using HireHub.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireHub.Data.EntitiesConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            const string ROLE_ID = "ef4c3c3e-8d45-4f36-92a8-994ce2d811fe";
            builder.HasData(new IdentityRole
            {

                Id = ROLE_ID,
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = ROLE_ID
            });
        }
    }
}
