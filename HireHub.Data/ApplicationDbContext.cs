﻿using HireHub.Data.Entities;
using HireHub.Data.EntitiesConfigurations;

namespace HireHub.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new JobEntityTypeConfiguration());
            builder.ApplyConfiguration(new ApplicationEntityTypeConfiguration());
            builder.ApplyConfiguration(new ApplicationResumeEntityTypeConfiguration());

            base.OnModelCreating(builder);
        }

        public DbSet<Job> Jobs { get; set; } = null!;
        public DbSet<Resume> Resumes { get; set; } = null!;
        public DbSet<Application> Applications { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<Town> Towns { get; set; } = null!;
        public DbSet<ApplicationResume> ApplicationResumes { get; set; } = null!;

    }
}