using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace HireHub.Data.EntitiesConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(Seed());
        }

        private List<IdentityRole> Seed()
        {
            const string ROLE_ID = "ef4c3c3e-8d45-4f36-92a8-994ce2d811fe";
            var role = new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Id = ROLE_ID,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = ROLE_ID
                },
            };

            return role;
        }
    }
}