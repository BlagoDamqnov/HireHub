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
            var isExistWithSameData = await _context.Companies.AnyAsync(c => c.Name == createCompanyVM.Name.Trim() || c.ContactEmail == createCompanyVM.ContactEmail || c.ContactPhone == createCompanyVM.ContactPhone);

            if (isExistWithSameData)
            {
                throw new ArgumentException("Company with same data already exist!");
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

        public async Task<EditCompanyVM> EditCompanyAsync(EditCompanyVM editCompanyVM, string userId)
        {
            var getCompanyByUserId = await _context.Companies.FirstOrDefaultAsync(c => c.UserId == userId);

            var isExistWithData = await _context.Companies.AnyAsync(c => c.Id != getCompanyByUserId!.Id && (c.Name == editCompanyVM.Name.Trim() || c.ContactEmail == editCompanyVM.ContactEmail || c.ContactPhone == editCompanyVM.ContactPhone));

            if (isExistWithData)
            {
                throw new ArgumentException("Company with same22 data already exist!");
            }

            if (string.IsNullOrWhiteSpace(editCompanyVM.Name) || string.IsNullOrWhiteSpace(editCompanyVM.ContactEmail) || string.IsNullOrWhiteSpace(editCompanyVM.ContactPhone))
            {
                throw new ArgumentException("You have not changed anything!");
            }
            if (getCompanyByUserId == null) 
            {
                throw new ArgumentException("Company not found!");
            }
            getCompanyByUserId.Name = editCompanyVM.Name.Trim();
            getCompanyByUserId.ContactEmail = editCompanyVM.ContactEmail.Trim();
            getCompanyByUserId.ContactPhone = editCompanyVM.ContactPhone!.Trim();

            await _context.SaveChangesAsync();

            return editCompanyVM;
        }

        public async Task<EditCompanyVM> GetCompanyByUserId(string userId)
        {
            var company = await _context.Companies.Where(c => c.UserId == userId)
                .Select(c => new EditCompanyVM()
                {
                    Id = c.Id,
                    Name = c.Name,
                    ContactEmail = c.ContactEmail,
                    ContactPhone = c.ContactPhone
                }).FirstOrDefaultAsync();

            return company!;
        }

        public async  Task<bool> DeleteCompany(int id)
        {
           var company = _context.Companies.FirstOrDefault(c => c.Id == id);
            if (company == null)
            {
                throw new ArgumentException("Company not found!");
            }
           
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return Task.CompletedTask.IsCompleted;
        }
    }
}
