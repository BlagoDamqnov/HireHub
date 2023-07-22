using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireHub.Data.EntitiesConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public class UserRoleEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasKey(x => new { x.UserId, x.RoleId });

            builder.HasData(Seed());
        }

        private List<IdentityUserRole<string>> Seed()
        {
            var userToRole = new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>
                {
                    RoleId = "ef4c3c3e-8d45-4f36-92a8-994ce2d811fe",
                    UserId = "4aa6831b-552e-473b-b40e-f71d5b8a5b44"
                },
            };

            return userToRole;
        }
    }
}