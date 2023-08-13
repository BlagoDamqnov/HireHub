using HireHub.Data;
using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.Services.Data.Models.House;
using HireHub.Web.ViewModels.Categories;
using HireHub.Web.ViewModels.Countries;
using HireHub.Web.ViewModels.Jobs;
using HireHub.Web.ViewModels.Jobs.Enums;
using HireHub.Web.ViewModels.Towns;
using Microsoft.EntityFrameworkCore;

namespace HireHub.Web.Services.Data
{
    using HireHub.Data.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class JobService : IJobService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICompanyService _companyService;

        public JobService(ApplicationDbContext context, ICompanyService companyService)
        {
            _context = context;
            _companyService = companyService;
        }

        public async Task<AllJobsFilteredServiceModel> GetJobs(AllJobsQueryModel queryModel)
        {
            IQueryable<Job> jobsQuery = this._context
                .Jobs
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryModel.Category))
            {
                jobsQuery = jobsQuery.Where(j => j.Category.CategoryName == queryModel.Category);
            }

            if (!string.IsNullOrWhiteSpace(queryModel.SearchString))
            {
                string wildCard = $"%{queryModel.SearchString.ToLower()}%";

                jobsQuery = jobsQuery
                    .Where(h => EF.Functions.Like(h.Title, wildCard) ||
                                EF.Functions.Like(h.Description, wildCard));
            }

            jobsQuery = queryModel.JobSorting switch
            {
                JobSorting.Newest => jobsQuery.OrderByDescending(h => h.CreatedOn),
                JobSorting.Oldest => jobsQuery.OrderBy(h => h.CreatedOn),
                JobSorting.SalaryDescending => jobsQuery.OrderByDescending(h => h.MinSalary),
                JobSorting.SalaryAscending => jobsQuery.OrderBy(h => h.MinSalary),
                _ => throw new NotImplementedException(),
            };

            IEnumerable<GetJobsVM> allJobs = await jobsQuery
                .Where(j => j.IsDeleted == false && j.IsApproved == true && j.Company.IsDeleted == false)
                .Select(j => new GetJobsVM()
                {
                    Id = j.Id,
                    Title = j.Title,
                    Town = j.Location.TownName,
                    CreatorId = j.CreatorId,
                    CompanyName = j.Company.Name,
                    Category = j.Category.CategoryName,
                    MinSalary = j.MinSalary,
                    MaxSalary = j.MaxSalary,
                    CreatedOn = j.CreatedOn,
                    LogoUrl = j.Company.LogoUrl
                })
                .Take(100)
                .ToArrayAsync();

            return new AllJobsFilteredServiceModel()
            {
                Jobs = allJobs
            };
        }

        public async Task<CreateJobVM> GetNewJobAsync()
        {
            var country = await _context.Countries
                .Select(c => new CountryVM()
                {
                    CountryId = c.Id,
                    Name = c.CountryName
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

        public async Task<IEnumerable<TownVM>> GetTownsByCountryId(int countryId)
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
                if (job.MinSalary >= job.MaxSalary)
                {
                    throw new InvalidOperationException("Min salary must be less than max salary");
                }
                var jobToAdd = new Job()
                {
                    Title = job.Title.Trim(),
                    Description = job.Description.Trim(),
                    MinSalary = job.MinSalary,
                    MaxSalary = job?.MaxSalary,
                    LocationId = job!.TownId,
                    CreatorId = creatorId,
                    CreatedOn = DateTime.UtcNow,
                    CategoryId = job.CategoryId,
                    IsDeleted = false,
                    IsApproved = false,
                    CompanyId = companyId,
                    Requirements = job.Requirements.Trim()
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

        public async Task<IEnumerable<GetJobsVM>> GetAllJobsForApprove()
        {
            var jobs = await _context.Jobs
                .Where(j => j.IsDeleted == false && j.IsApproved == false && j.Company.IsDeleted == false)
                .Select(j => new GetJobsVM()
                {
                    Id = j.Id,
                    Title = j.Title,
                    Town = j.Location.TownName,
                    CreatorId = j.CreatorId,
                    CompanyName = j.Company.Name,
                    MinSalary = j.MinSalary,
                    MaxSalary = j.MaxSalary,
                    CreatedOn = j.CreatedOn,
                    LogoUrl = j.Company.LogoUrl
                })
                .OrderByDescending(j => j.CreatedOn)
                .ToListAsync();

            return jobs;
        }

        public async Task ApproveJob(string id)
        {
            var job = _context.Jobs.FirstOrDefault(j => j.Id == Guid.Parse(id));

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

        public async Task RejectJob(string id)
        {
            var job = _context.Jobs.FirstOrDefault(j => j.Id == Guid.Parse(id));

            if (job != null)
            {
                job.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Job not found");
            }
        }

        public async Task<DetailsJobVM?> GetJobDetails(string id)
        {
            var parsedJobId = Guid.Parse(id);
            var job = await _context.Jobs
                .Where(j => j.Id == parsedJobId)
                .Select(j => new DetailsJobVM()
                {
                    Id = j.Id,
                    CreatorId = j.CreatorId,
                    Title = j.Title,
                    PhoneNumber = j.Company.ContactPhone,
                    Description = j.Description,
                    MinSalary = j.MinSalary,
                    MaxSalary = j.MaxSalary,
                    Location = j.Location.TownName,
                    Category = j.Category.CategoryName,
                    CompanyName = j.Company.Name,
                    LogoUrl = j.Company.LogoUrl,
                    Requirements = j.Requirements,
                    CreatedOn = j.CreatedOn
                })
                .FirstOrDefaultAsync();

            return job;
        }

        public async Task DeleteJob(string id, string userId)
        {
            var parsedJobId = Guid.Parse(id);
            var isExist = await _context.Jobs.AnyAsync(j => j.Id == parsedJobId);

            if (isExist)
            {
                var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == parsedJobId);
                if (job!.CreatorId != userId)
                {
                    throw new InvalidOperationException("You are not creator of this job");
                }
                else
                {
                    job!.IsDeleted = true;
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                throw new InvalidOperationException("Job not found");
            }
        }

        public async Task<EditJobVM> GetJobDetailsForEdit(string id, string userId)
        {
            var isExist = await _context.Jobs.AnyAsync(j => j.Id == Guid.Parse(id));
            if (isExist)
            {
                var getJobForCheck = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == Guid.Parse(id));

                if (getJobForCheck?.CreatorId != userId)
                {
                    throw new InvalidOperationException("You are not creator of this job");
                }
                var job = await _context.Jobs.Where(j => j.Id == Guid.Parse(id))
                   .Select(j => new EditJobVM()
                   {
                       Id = j.Id,
                       Title = j.Title,
                       CategoryId = j.CategoryId,
                       TownName = j.Location.TownName,
                       CountryName = j.Location.Country.CountryName,
                       CountryId = j.Location.CountryId,
                       TownId = j.LocationId,
                       Description = j.Description,
                       MinSalary = j.MinSalary,
                       MaxSalary = j.MaxSalary,
                       Requirements = j.Requirements
                   })
                   .FirstOrDefaultAsync();

                var categories = await _context.Categories
                    .Select(c => new CategoryVM()
                    {
                        Id = c.Id,
                        Name = c.CategoryName
                    })
                    .ToListAsync();

                job!.Categories = categories;

                var countries = await _context.Countries
                    .Select(c => new CountryVM()
                    {
                        CountryId = c.Id,
                        Name = c.CountryName
                    })
                    .ToListAsync();

                job!.Countries = countries;

                job!.Towns = await _context.Towns.Where(t => t.CountryId == job.CountryId)
                    .Select(t => new TownVM()
                    {
                        TownId = t.Id,
                        Name = t.TownName
                    })
                    .ToListAsync();

                return job!;
            }
            return null!;
        }

        public async Task EditJob(EditJobVM model)
        {
            var jobForEdit = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == model.Id && j.IsDeleted == false);

            if (jobForEdit == null)
            {
                throw new InvalidOperationException("Job not found");
            }

            if (model.MinSalary >= model.MaxSalary)
            {
                throw new InvalidOperationException("Min salary must be less than max salary");
            }
            jobForEdit.Title = model.Title.Trim();
            jobForEdit.CategoryId = model.CategoryId;
            jobForEdit.LocationId = model.TownId;
            jobForEdit.Description = model.Description.Trim();
            jobForEdit.MinSalary = model.MinSalary;
            jobForEdit.MaxSalary = model.MaxSalary;
            jobForEdit.Requirements = model.Requirements.Trim();

            await _context.SaveChangesAsync();
        }

        public async Task<AllJobsFilteredServiceModel> GetJobsByCompanyId(int companyId, AllJobsQueryModel queryModel)
        {
            IQueryable<Job> jobsQuery = this._context
                .Jobs.Where(j => j.CompanyId == companyId && j.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryModel.Category))
            {
                jobsQuery = jobsQuery.Where(j => j.Category.CategoryName == queryModel.Category);
            }

            if (!string.IsNullOrWhiteSpace(queryModel.SearchString))
            {
                string wildCard = $"%{queryModel.SearchString.ToLower()}%";

                jobsQuery = jobsQuery
                    .Where(h => EF.Functions.Like(h.Title, wildCard) ||
                                EF.Functions.Like(h.Description, wildCard));
            }

            jobsQuery = queryModel.JobSorting switch
            {
                JobSorting.Newest => jobsQuery.OrderByDescending(h => h.CreatedOn),
                JobSorting.Oldest => jobsQuery.OrderBy(h => h.CreatedOn),
                JobSorting.SalaryDescending => jobsQuery.OrderByDescending(h => h.MinSalary),
                JobSorting.SalaryAscending => jobsQuery.OrderBy(h => h.MinSalary),
                _ => throw new NotImplementedException(),
            };

            IEnumerable<GetJobsVM> allJobs = await jobsQuery
                .Where(j => j.IsDeleted == false && j.IsApproved == true && j.Company.IsDeleted == false)
                .Select(j => new GetJobsVM()
                {
                    Id = j.Id,
                    Title = j.Title,
                    Town = j.Location.TownName,
                    CreatorId = j.CreatorId,
                    CompanyName = j.Company.Name,
                    Category = j.Category.CategoryName,
                    MinSalary = j.MinSalary,
                    MaxSalary = j.MaxSalary,
                    CreatedOn = j.CreatedOn,
                    LogoUrl = j.Company.LogoUrl
                })
                .Take(100)
                .ToArrayAsync();

            return new AllJobsFilteredServiceModel()
            {
                Jobs = allJobs
            };
        }
    }
}