using HireHub.Data;
using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Categories;
using HireHub.Web.ViewModels.Countries;
using HireHub.Web.ViewModels.Jobs;
using HireHub.Web.ViewModels.Towns;
using Microsoft.EntityFrameworkCore;

namespace HireHub.Web.Services.Data
{
    using HireHub.Data.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class JobService:IJobService
    {
        private readonly ApplicationDbContext _context;

        public JobService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetLastFiveJobsVM>> GetLastFiveJobs()
        {
            var jobs = await _context.Jobs
                .Select(j => new GetLastFiveJobsVM()
                {
                    Id = j.Id,
                    Title = j.Title,
                    Town = j.Location.TownName,
                    CompanyName = j.User.UserName,
                    MinSalary = j.MinSalary,
                    MaxSalary = j.MaxSalary,
                    CreatedOn = j.CreatedOn,
                    LogoUrl = j.LogoUrl
                })
                .OrderByDescending(j => j.CreatedOn)
                .ToListAsync();

            return jobs;
        }

        public async Task<CreateJobVM> GetNewJobAsync()
        {
            var country = await _context.Countries
                .Select(c => new CountryVM()
                {
                    CountryId = c.Id,
                    Name =c.CountryName
                })
                .ToListAsync();

            var categories = await _context.Categories
                .Select(c => new CategoryVM()
                {
                    Id = c.Id,
                    Name = c.CategoryName
                })
                .ToListAsync();

            var job = new CreateJobVM()
            {
                Countries = country,
                Categories = categories
            };

            return job;
        }

        public async Task<CreateJobVM> GetTownsByCountryId(CreateJobVM job , int countryId)
        {
            var towns = await _context.Towns
                .Where(t => t.CountryId == countryId)
                .Select(t => new TownVM()
                {
                    TownId = t.Id,
                    Name = t.TownName
                })
                .ToListAsync();

             job.Towns = towns;

            return job;
        }

        public async Task AddJobAsync(CreateJobVM job, string creatorId)
        {
            var jobToAdd = new Job()
            {
                Title = job.Title,
                Description = job.Description,
                MinSalary = job.MinSalary,
                MaxSalary = job?.MaxSalary,
                LocationId = job.TownId,
                CreatorId = creatorId,
                CreatedOn = DateTime.UtcNow,
                LogoUrl = job.Logo,
                CategoryId = job.CategoryId,
                IsDeleted = false,
                IsApproved = true,
                Requirements = job.Requirements
            };

            await _context.Jobs.AddAsync(jobToAdd);
            await _context.SaveChangesAsync();
        }
    }
}
