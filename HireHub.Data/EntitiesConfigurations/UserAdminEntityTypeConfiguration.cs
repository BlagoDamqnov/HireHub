using HireHub.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireHub.Data.EntitiesConfigurations
{
    public class UserAdminEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasData(Seed());
        }

        private ApplicationUser Seed()
        {
            var user = new ApplicationUser()
            {
                Id = "4aa6831b-552e-473b-b40e-f71d5b8a5b44",
                Email = "admin@abv.bg",
                EmailConfirmed = true,
                UserName = "admin@abv.bg",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                NormalizedUserName = "ADMIN@ABV.BG"
            };

            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = ph.HashPassword(user, "000000");

            return user;
        }
    }
}