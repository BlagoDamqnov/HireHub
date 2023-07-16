using HireHub.Data;
using HireHub.Data.Entities;
using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Resume;
using Microsoft.EntityFrameworkCore;

namespace HireHub.Web.Services.Data
{
    using HireHub.Web.ViewModels.Application;
    using HireHub.Web.ViewModels.Company;
    using HireHub.Web.ViewModels.Jobs;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ApplicationService : IApplicationService
    {
        private readonly ApplicationDbContext _context;


        public ApplicationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApplyForJobVM> AddApplicationAsync(string userId,string jobId)
        {
            var isOwner = await _context.Jobs
    .AnyAsync(x => x.Id == Guid.Parse(jobId) && x.CreatorId == userId);

            if (isOwner)
            {
                throw new InvalidOperationException("You can't apply for your own job!");
            }
            var resumes = await _context.Resumes
                .Select(r => new GetResumeVM()
                {
                    Id = r.Id,
                    Name = r.Name.Trim(),
                    ResumePath = r.ResumePath.Trim(),
                    CreatorId = r.CreatorId
                }).Where(x => x.CreatorId == userId).ToListAsync();

            var app = new ApplyForJobVM()
            {
                Resumes = resumes
            };

            return app;
        }

        public async Task AddApply(ApplyForJobVM model, string jobId, string userId)
        {
            var parseJobId = Guid.Parse(jobId);

            var isApply = await _context.Applications
                .Where(x => x.ApplierId == userId && x.JobId == parseJobId && x.IsDeleted == false).AnyAsync();


            if (isApply == false)
            {
                var application = new Application
                {
                    JobId = parseJobId,
                    ResumeId = model.ResumeId,
                    IsApproved = true,
                    ApplierId = userId,
                    CreatedOn = DateTime.UtcNow
                };

                await _context.Applications.AddAsync(application);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("You have already applied for this job!");
            }

        }

        public async Task<IEnumerable<GetAllApplications>> GetMyApplication(string userId)
        {
            var getAllApplications = await _context.Applications
                 .Where(j => j.ApplierId == userId && j.IsDeleted == false)
                 .Select(j => new GetAllApplications()
                 {
                     Id = j.Job.Id,
                     Title = j.Job.Title,
                     CreatedOn = j.Job.CreatedOn,
                     LogoUrl = j.Job.Company.LogoUrl,
                     Resume = j.Resume.ResumePath
                 }).ToListAsync();

            return getAllApplications;
        }

        public Task RemoveApplication(string id, string userId)
        {
           var parseId = Guid.Parse(id);
            var application = _context.Applications
                .Where(x => x.ApplierId == userId && x.JobId == parseId && x.IsDeleted == false)
                .FirstOrDefault();

            if (application != null)
            {
                application.IsDeleted = true;
                _context.Applications.Update(application);
                _context.SaveChanges();
            }

            return Task.CompletedTask;
        }
    }
}
