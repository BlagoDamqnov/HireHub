using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace HireHub.Data.EntitiesConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UserRoleEntityTypeConfiguration:IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasKey(x => new { x.UserId, x.RoleId });

            builder.HasData(new IdentityUserRole<string>
            {
                RoleId = "ef4c3c3e-8d45-4f36-92a8-994ce2d811fe",
                UserId = "ff7240f7-02d0-4d9f-aab9-43b759a328df"
            });
        }
    }
}
