using HireHub.Data;
using HireHub.Data.Entities;
using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Resume;

namespace HireHub.Web.Services.Data
{
    using System.Threading.Tasks;

    public class ResumeService : IResumeService
    {
        private readonly ApplicationDbContext _context;

        public ResumeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddResumeAsync(AddResumeVM model, string userId)
        {
            var resume = new Resume
            {
                Name = model.Name.Trim(),
                ResumePath = model.ResumePath.Trim(),
                CreatorId = userId
            };

            await _context.Resumes.AddAsync(resume);
            await _context.SaveChangesAsync();
        }
    }
}