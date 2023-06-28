using HireHub.Data;
using HireHub.Data.Entities;
using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Resume;
using Microsoft.EntityFrameworkCore;

namespace HireHub.Web.Services.Data
{
    using HireHub.Web.ViewModels.Application;
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

        public async Task<ApplyForJobVM> AddApplicationAsync(string userId)
        {
            var resumes = await _context.Resumes
                .Select(r => new GetResumeVM()
                {
                    Id = r.Id,
                    Name = r.Name,
                    ResumePath = r.ResumePath,
                    CreatorId = r.CreatorId
                }).Where(x => x.CreatorId == userId).ToListAsync();

            var app = new ApplyForJobVM()
            {
                Resumes = resumes
            };

            return app;
        }

        public async Task AddApply(ApplyForJobVM model,Guid jobId,string userId)
        {
            var isApply = await _context.Applications
                .Where(x => x.ApplierId == userId && x.JobId == jobId)
                .AnyAsync(x =>x.IsDeleted == true || x.IsApproved == true);

            if (isApply == false)
            {
                var application = new Application
                {
                    JobId = jobId,
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
    }
}
