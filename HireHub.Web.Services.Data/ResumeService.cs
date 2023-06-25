using HireHub.Data;
using HireHub.Data.Entities;
using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Resume;

namespace HireHub.Web.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ResumeService:IResumeService
    {
        private readonly ApplicationDbContext _context;

        public ResumeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddResumeAsync(AddResumeVM model,string userId)
        {
            var resume = new Resume
            {
                Name = model.Name,
                ResumePath = model.ResumePath,
                CreatorId = userId
            };

            await _context.Resumes.AddAsync(resume);
            await _context.SaveChangesAsync();
        }
        
    }
}
