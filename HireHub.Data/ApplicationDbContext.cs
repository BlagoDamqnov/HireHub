using HireHub.Data.Entities;
using HireHub.Data.EntitiesConfigurations;
using HireHub.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace HireHub.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CountryEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserAdminEntityTypeConfiguration());
            builder.ApplyConfiguration(new TownEntityTypeConfiguration());
            builder.ApplyConfiguration(new JobEntityTypeConfiguration());
            builder.ApplyConfiguration(new ApplicationEntityTypeConfiguration());
            builder.ApplyConfiguration(new CompanyEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserRoleEntityTypeConfiguration());
            base.OnModelCreating(builder);
        }

        public DbSet<Job> Jobs { get; set; } = null!;
        public DbSet<Resume> Resumes { get; set; } = null!;
        public DbSet<Application> Applications { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<Town> Towns { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;

    }
}