﻿using HireHub.Data;
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
                .Where(j => j.IsDeleted == false && j.IsApproved == true)
                .Select(j => new GetLastFiveJobsVM()
                {
                    Id = j.Id,
                    Title = j.Title,
                    Town = j.Location.TownName,
                    CompanyName = j.Company.Name,
                    MinSalary = j.MinSalary,
                    MaxSalary = j.MaxSalary,
                    CreatedOn = j.CreatedOn,
                    LogoUrl = j.LogoUrl
                })
                .OrderByDescending(j => j.CreatedOn)
                .Take(10)
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

        public async Task<IEnumerable<TownVM>> GetTownsByCountryId( int countryId)
        {
            var towns = await _context.Towns
                .Where(t => t.CountryId == countryId)
                .Select(t => new TownVM()
                {
                    TownId = t.Id,
                    Name = t.TownName
                })
                .ToListAsync();

            return towns;
        }

        public async Task AddJobAsync(CreateJobVM job, string creatorId, int companyId)
        {
            try
            {
                var jobToAdd = new Job()
                {
                    Title = job.Title,
                    Description = job.Description,
                    LogoUrl = job.Logo,
                    MinSalary = job.MinSalary,
                    MaxSalary = job?.MaxSalary,
                    LocationId = job.TownId,
                    CreatorId = creatorId,
                    CreatedOn = DateTime.UtcNow,
                    CategoryId = job.CategoryId,
                    IsDeleted = false,
                    IsApproved = false,
                    CompanyId = companyId,
                    Requirements = job.Requirements
                };

                _context.Jobs.Add(jobToAdd);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }


        public async Task<IEnumerable<GetLastFiveJobsVM>> GetAllJobsForApprove()
        {
            var jobs = await _context.Jobs
                .Where(j => j.IsDeleted == false && j.IsApproved == false)
                .Select(j => new GetLastFiveJobsVM()
                {
                    Id = j.Id,
                    Title = j.Title,
                    Town = j.Location.TownName,
                    CompanyName = j.Company.Name,
                    MinSalary = j.MinSalary,
                    MaxSalary = j.MaxSalary,
                    CreatedOn = j.CreatedOn,
                    LogoUrl = j.LogoUrl
                })
                .OrderByDescending(j => j.CreatedOn)
                .ToListAsync();

            return jobs;
        }

        public async Task ApproveJob(Guid id)
        {
            var job = _context.Jobs.FirstOrDefault(j => j.Id == id);

            if (job != null)
            {
                job.IsApproved = true;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Job not found");
            }
        }

        public async Task RejectJob(Guid id)
        {
            var job = _context.Jobs.FirstOrDefault(j => j.Id == id);

            if(job != null)
            {
                job.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Job not found");
            }
        }

        public async Task<DetailsJobVM?> GetJobDetails(Guid id)
        {
            var job = await _context.Jobs
                .Where(j => j.Id == id)
                .Select(j => new DetailsJobVM()
                {
                    Id = j.Id,
                    CreatorId = j.CreatorId,
                    Title = j.Title,
                    Description = j.Description,
                    MinSalary = j.MinSalary,
                    MaxSalary = j.MaxSalary,
                    Location = j.Location.TownName,
                    Category = j.Category.CategoryName,
                    CompanyName = j.Company.Name,
                    LogoUrl = j.LogoUrl,
                    Requirements = j.Requirements,
                    CreatedOn = j.CreatedOn
                })
                .FirstOrDefaultAsync();

            return job;
        }

        public async Task DeleteJob(Guid id)
        {
            var isExist = await _context.Jobs.AnyAsync(j => j.Id == id);

            if (isExist)
            {
                var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id);
                job.IsDeleted = true;
               await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Job not found");
            }
        }

        public async Task<IEnumerable<GetLastFiveJobsVM>> SearchJobs(string search)
        {
            var result = await _context.Jobs
                .Where(j => (j.Title.Contains(search) || j.Description.Contains(search)) && j.IsApproved == true)
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
                .Take(10)
                .ToListAsync();
            return result;
        }
    }
}
