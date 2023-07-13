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
            return await _context.Companies.Where(c => c.UserId == userId && c.IsDeleted == false)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserHaveCompany(string userId)
        {
            var company = await _context.Companies.AnyAsync(c => c.UserId == userId && c.IsDeleted == false);

            return company;
        }

        public async Task<string?> GetCompanyNameByUserId(string id)
        {
            return await _context.Companies.Where(c => c.UserId == id && c.IsDeleted == false)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<GetAllApplications>> MyApplication(int companyId)
        {
            var app = await _context.Applications
                .Where(j => j.Job.CompanyId == companyId && j.IsDeleted == false && j.Job.Company.IsDeleted == false)
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
            var getCompanyByUserId = await _context.Companies.FirstOrDefaultAsync(c => c.UserId == userId && c.IsDeleted == false);

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
            var company = await _context.Companies.Where(c => c.UserId == userId && c.IsDeleted == false)
                .Select(c => new EditCompanyVM()
                {
                    Id = c.Id,
                    Name = c.Name,
                    ContactEmail = c.ContactEmail,
                    ContactPhone = c.ContactPhone
                }).FirstOrDefaultAsync();

            return company!;
        }

        public async Task<bool> DeleteCompany(int id)
        {
            var company = _context.Companies.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (company == null)
            {
                throw new ArgumentException("Company not found!");
            }

           company.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Task.CompletedTask.IsCompleted;
        }

        public async Task HireUser(string userId, string jobId)
        {
           var hiring = new HiringRecord()
           {
               CandidateId = userId,
               JobId = Guid.Parse(jobId),
               DateOfHiring = DateTime.Now,
               IsHired = true
           };

            await _context.HiringRecords.AddAsync(hiring);
            await _context.SaveChangesAsync();
        }

        public async Task<bool?> IsHire(string userId, string jobId)
        {
           var user =  await _context.HiringRecords.FirstOrDefaultAsync(h=>h.JobId == Guid.Parse(jobId) && h.CandidateId == userId);

           if(user != null)
            {
               return user.IsHired;
           }

            return null;
        }

        public async Task<string> GetUserIdByEmail(string? email)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x=>x.Email.ToLower().Trim() == email!.ToLower().Trim());

            return user!.Id;
        }

        public async Task RejectUser(string userId, string jobId)
        {
            var hiring = new HiringRecord()
            {
                CandidateId = userId,
                JobId = Guid.Parse(jobId),
                DateOfHiring = DateTime.Now,
                IsHired = false
            };

            await _context.HiringRecords.AddAsync(hiring);
            await _context.SaveChangesAsync();
        }
    }
}
