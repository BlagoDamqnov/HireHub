using HireHub.Data;
using HireHub.Data.Models.Entities;
using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Company;
using HireHub.Web.ViewModels.Jobs;
using HireHub.Web.ViewModels.Users;
using Microsoft.EntityFrameworkCore;

namespace HireHub.Web.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _context;

        public CompanyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateCompanyAsync(CreateCompanyVM createCompanyVM, string userId)
        {
            var isExistWithSameName = await _context.Companies.AnyAsync(c => c.Name == createCompanyVM.Name.Trim());

           if(isExistWithSameName)
            {
                throw new ArgumentException("Company with same name already exist!");
            }
            else
            {
                var company = new Company()
                {
                    Name = createCompanyVM.Name.Trim(),
                    ContactEmail = createCompanyVM.ContactEmail.Trim(),
                    ContactPhone = createCompanyVM.ContactPhone!.Trim(),
                    UserId = userId
                };

                await _context.Companies.AddAsync(company);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetCompanyIdByUserId(string userId)
        {
            return await _context.Companies.Where(c => c.UserId == userId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserHaveCompany(string userId)
        {
            var company = await _context.Companies.AnyAsync(c => c.UserId == userId);

            return company;
        }

        public async Task<string?> GetCompanyNameByUserId(string id)
        {
            return await _context.Companies.Where(c => c.UserId == id)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<GetAllApplications>> MyApplication(int companyId)
        {
            var app = await _context.Applications
                .Where(j => j.Job.CompanyId == companyId && j.IsDeleted == false)
                .Select(j => new GetAllApplications()
                {
                    Id = j.Job.Id,
                    Title = j.Job.Title,
                    CreatedOn = j.Job.CreatedOn,
                    LogoUrl = j.Job.LogoUrl,
                    Username = j.ApplicationUser.UserName,
                    Resume = j.Resume.ResumePath
                }).ToListAsync();

            return app;

        }
    }
}
