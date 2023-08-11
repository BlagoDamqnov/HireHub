using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Identity;
using HireHub.Data;
using HireHub.Web.Services.Data;
using HireHub.Data.Models.Entities;
using HireHub.Web.ViewModels.Company;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using HireHub.Data.Entities;

namespace HireHub.Web.Tests
{
    public class CompanyServiceTests
    {
        private ApplicationDbContext context;
        private CompanyService _companyService;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        [SetUp]
        public void SetUp()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "TestDatabase")
                 .Options;

            this.context = new ApplicationDbContext(contextOptions);

            // Mock UserManager and SignInManager
            var userStore = new Mock<IUserStore<ApplicationUser>>();

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStore.Object, null, null, null, null, null, null, null, null);

            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                mockUserManager.Object, new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object, null, null, null, null);

            _userManager = mockUserManager.Object;
            _signInManager = mockSignInManager.Object;

            this._companyService = new CompanyService(context, _userManager, _signInManager);

        }


        [TearDown]
        public void TearDown()
        {
            this.context.Database.EnsureDeleted();
        }

        [Test]
        public async Task CreateCompanyAsync_DuplicateName_ThrowsArgumentException()
        {
            // Arrange
            var existingCompany = new Company
            {
                Name = "DuplicateCompany",
                LogoUrl = "https://www.google.com",
                ContactEmail = "blago33@gmail.com",
                ContactPhone = "0888888888",
                UserId = "1"
            };
            await context.Companies.AddAsync(existingCompany);
            await context.SaveChangesAsync();

            Assert.ThrowsAsync<ArgumentException>(async () => await _companyService.CreateCompanyAsync(new CreateCompanyVM
            {
                Name = "DuplicateCompany",
                LogoUrl = "https://www.google.com",
                ContactEmail = "koko@abv.bg"
            }, "1"));
        }

        [Test]
        public async Task GetCompanyByUserId_Succ()
        {
            var company = new Company
            {
                Name = "DuplicateCompany",
                LogoUrl = "https://www.google.com",
                ContactEmail = "koko@abv.bg",
                UserId = "1",
                ContactPhone = "0888888888"
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();

            var companyByUserId = await _companyService.GetCompanyByUserId("1");

            Assert.That(companyByUserId, Is.Not.Null);
            Assert.That(companyByUserId.Name, Is.EqualTo(company.Name));

        }

        [Test]
        public async Task IsUserHaveCompany_With_Valid_Id()
        {
            var company = new Company
            {
                Name = "DuplicateCompany",
                LogoUrl = "https://www.google.com",
                ContactEmail = "koko@abv.bg",
                UserId = "1",
                ContactPhone = "0888888888"
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();

            var isUserHaveCompany = await _companyService.IsUserHaveCompany("1");

            Assert.That(isUserHaveCompany, Is.True);
        }

        [Test]
        public async Task IsUserHaveCompany_With_Invalid_Id()
        {
            var company = new Company
            {
                Name = "DuplicateCompany",
                LogoUrl = "https://www.google.com",
                ContactEmail = "koko@abv.bg",
                UserId = "1",
                ContactPhone = "0888888888"
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();

            var isUserHaveCompany = await _companyService.IsUserHaveCompany("100");

            Assert.That(isUserHaveCompany, Is.False);
        }
        [Test]
        public async Task IsUserHaveCompany_With_Deleted_Company()
        {
            var company = new Company
            {
                Name = "DuplicateCompany",
                LogoUrl = "https://www.google.com",
                ContactEmail = "koko@abv.bg",
                UserId = "1",
                ContactPhone = "0888888888"
            };

            await context.Companies.AddAsync(company);
            company.IsDeleted = true;
            await context.SaveChangesAsync();

            var isUserHaveCompany = await _companyService.IsUserHaveCompany("1");

            Assert.IsFalse(isUserHaveCompany);
        }
        [Test]
        public async Task GetCompanyIdByUserId_With_Valid_Company_Id()
        {
            var company = new Company
            {
                Name = "DuplicateCompany",
                LogoUrl = "https://www.google.com",
                ContactEmail = "koko@abv.bg",
                UserId = "1",
                ContactPhone = "0888888888"
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();

            var companyIdByUserId = await _companyService.GetCompanyIdByUserId("1");
            var companyId = await context.Companies.Select(x => x.Id).FirstOrDefaultAsync();
            Assert.That(companyId, Is.EqualTo(companyIdByUserId));
        }

        [Test]
        public async Task GetCompanyIdByUserId_With_Delete_Company()
        {
            var company = new Company
            {
                Name = "DuplicateCompany",
                LogoUrl = "https://www.google.com",
                ContactEmail = "koko@abv.bg",
                UserId = "1",
                ContactPhone = "0888888888"
            };

            await context.Companies.AddAsync(company);
            company.IsDeleted = true;
            await context.SaveChangesAsync();

            var companyIdByUserId = await _companyService.GetCompanyIdByUserId("1");
            Assert.That(companyIdByUserId, Is.EqualTo(0));
        }

        [Test]
        public async Task GetCompanyNameByUserId_With_Valid_UserId()
        {
            var company = new Company
            {
                Name = "DuplicateCompany",
                LogoUrl = "https://www.google.com",
                ContactEmail = "koko@abv.bg",
                UserId = "1",
                ContactPhone = "0888888888"
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();

            var companyName = await _companyService.GetCompanyNameByUserId("1");

            Assert.That(companyName, Is.EqualTo(company.Name));
        }
        [Test]
        public async Task GetCompanyNameByUserId_With_Invalid_UserId()
        {
            var company = new Company
            {
                Name = "DuplicateCompany",
                LogoUrl = "https://www.google.com",
                ContactEmail = "koko@abv.bg",
                UserId = "1",
                ContactPhone = "0888888888"
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();

            var companyName = await _companyService.GetCompanyNameByUserId("199");

            Assert.IsNull(companyName);
        }

        [Test]
        public async Task GetCompanyNameByUserId_With_Deleted_Company()
        {
            var company = new Company
            {
                Name = "DuplicateCompany",
                LogoUrl = "https://www.google.com",
                ContactEmail = "koko@abv.bg",
                UserId = "1",
                ContactPhone = "0888888888"
            };

            await context.Companies.AddAsync(company);
            company.IsDeleted = true;
            await context.SaveChangesAsync();

            var companyName = await _companyService.GetCompanyNameByUserId("1");

            Assert.IsNull(companyName);
        }
        [Test]
        public async Task MyApplication_Succ_ReturnsApplications()
        {
            // Test setup
            var company = new Company
            {
                Id = 23,
                Name = "DuplicateCompany",
                LogoUrl = "https://www.google.com",
                ContactEmail = "koko@abv.bg",
                ContactPhone = "0888888888",
                UserId = "1",
                IsDeleted = false,
            };
            var job = new Job
            {
                Id = Guid.NewGuid(),
                CompanyId = company.Id,
                Title = "Sample Job Title",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                Location = new Town { TownName = "Sample Town" },
                CreatorId = "1",
                Company = company,
                Category = new Category { CategoryName = "Sample Category" },
                MinSalary = 30000,
                MaxSalary = 50000,
                CreatedOn = DateTime.Now.AddDays(-7),
                IsDeleted = false,
                IsApproved = true,
            };
            var resume = new Resume
            {
                Id = 1,
                Name = "Sample Resume",
                CreatorId = Guid.NewGuid().ToString(),
                ResumePath = "Sample Path",
            };
            var application = new Application
            {
                Id = Guid.NewGuid(),
                JobId = job.Id,
                ResumeId = resume.Id,
                ApplierId = resume.CreatorId,
                Job = job,
                Resume = resume,
                CreatedOn = DateTime.Now.AddDays(-7),
                IsDeleted = false,
            };
            await context.Companies.AddAsync(company);
            await context.Jobs.AddAsync(job);
            await context.Resumes.AddAsync(resume);
            await context.Applications.AddAsync(application);
            await context.SaveChangesAsync();
        }


        [Test]
        public async Task EditCompanyAsync_UniqueData_CompanyEdited()
        {
            // Test setup
            var existingCompany = new Company
            {
                Id = 1,
                UserId = "user123",
                Name = "OriginalCompany",
                LogoUrl = "original_logo.png",
                ContactEmail = "original@company.com",
                ContactPhone = "1234567890"
            };
            await context.Companies.AddAsync(existingCompany);
            await context.SaveChangesAsync();

            var editCompanyVM = new EditCompanyVM
            {
                Name = "EditedCompany",
                LogoUrl = "edited_logo.png",
                ContactEmail = "edited@company.com",
                ContactPhone = "9876543210"
            };

            // Test action
            await _companyService.EditCompanyAsync(editCompanyVM, "user123");

            // Assert
            var editedCompany = await context.Companies.FindAsync(1);
            Assert.NotNull(editedCompany);
            Assert.That(editedCompany.Name, Is.EqualTo(editCompanyVM.Name.Trim()));
            Assert.That(editedCompany.LogoUrl, Is.EqualTo(editCompanyVM.LogoUrl.Trim()));
            Assert.That(editedCompany.ContactEmail, Is.EqualTo(editCompanyVM.ContactEmail.Trim()));
            Assert.That(editedCompany.ContactPhone, Is.EqualTo(editCompanyVM.ContactPhone.Trim()));
        }
        [Test]
        public async Task EditCompanyAsync_DuplicateData_ThrowsArgumentException()
        {
            // Test setup
            var existingCompany = new Company
            {
                Id = 1,
                UserId = "user123",
                Name = "OriginalCompany",
                LogoUrl = "original_logo.png",
                ContactEmail = "original@company.com",
                ContactPhone = "1234567890"
            };
            await context.Companies.AddAsync(existingCompany);
            await context.SaveChangesAsync();

            var editCompanyVM = new EditCompanyVM
            {
                Name = "OriginalCompany", // Duplicate name
                LogoUrl = "edited_logo.png",
                ContactEmail = "original@company.com", // Duplicate email
                ContactPhone = "9876543210"
            };

            // Test action and assert
            var result = await _companyService.EditCompanyAsync(editCompanyVM, "user123");

            Assert.That(editCompanyVM, Is.EqualTo(result));
        }

        [Test]
        public async Task EditCompanyAsync_MissingData_ThrowsArgumentException()
        {
            // Test setup
            var existingCompany = new Company
            {
                Id = 1,
                UserId = "user123",
                Name = "OriginalCompany",
                LogoUrl = "original_logo.png",
                ContactEmail = "original@company.com",
                ContactPhone = "1234567890"
            };
            await context.Companies.AddAsync(existingCompany);
            await context.SaveChangesAsync();

            var editCompanyVM = new EditCompanyVM
            {
                Name = "EditedCompany",
                LogoUrl = "edited_logo.png",
                ContactEmail = "", // Missing email
                ContactPhone = "9876543210"
            };

            // Test action and assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _companyService.EditCompanyAsync(editCompanyVM, "user123"));

            Assert.That(exception.Message, Is.EqualTo("You have not changed anything!"));
        }

        [Test]
        public async Task EditCompanyAsync_With_Exist_Data_ThrowsArgumentException()
        {
            var company = new Company
            {
                Id = 2,
                UserId = "user12223",
                Name = "OriginalCompany99",
                LogoUrl = "original_logo.png",
                ContactEmail = "ddd@abv.bg",
                ContactPhone = "1234567890"
            };
            // Test setup
            var existingCompany = new Company
            {
                Id = 1,
                UserId = "user123",
                Name = "OriginalCompany",
                LogoUrl = "original_logo.png",
                ContactEmail = "original@company.com",
                ContactPhone = "1234567890"
            };
            await context.Companies.AddAsync(company);
            await context.Companies.AddAsync(existingCompany);
            await context.SaveChangesAsync();

            var editCompanyVM = new EditCompanyVM
            {
                Name = "OriginalCompany99", // Duplicate name
                LogoUrl = "edited_logo.png",
                ContactEmail = "original@company.com", // Duplicate email
                ContactPhone = "9876543210"
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
                           await _companyService.EditCompanyAsync(editCompanyVM, "user123"));
        }

        [Test]
        public async Task DeleteCompany_ValidId_CompanyDeletedAndClaimsUpdated()
        {
            var company = new Company
            {
                Id = 1,
                UserId = "user123",
                Name = "TestCompany",
                LogoUrl = "test_logo.png",
                ContactEmail = "ddd@abv.bg",
                IsDeleted = false
            };
            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();

            var user = new ApplicationUser
            {
                Id = "user123"
            };

            Mock.Get(_userManager).Setup(m => m.FindByIdAsync("user123")).ReturnsAsync(user);
            Mock.Get(_userManager).Setup(m => m.GetClaimsAsync(user))
                .ReturnsAsync(new List<Claim>());
            await _companyService.DeleteCompany(1);

            var deletedCompany = await context.Companies.FindAsync(1);

            Assert.NotNull(deletedCompany);
            Assert.IsTrue(deletedCompany.IsDeleted);
        }

        [Test]
        public async Task DeleteCompany_NonExistentId_ThrowsArgumentException()
        {
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _companyService.DeleteCompany(999));

            Assert.That(exception.Message, Is.EqualTo("Company not found!"));
        }

        [Test]
        public async Task HireUser_Succ()
        {
            var company = new Company
            {
                Id = 111,
                UserId = "user123",
                Name = "TestCompany",
                LogoUrl = "test_logo.png",
                ContactEmail = "koko@abv.bg",
                IsDeleted = false,
                ContactPhone = "1234567890"
            };

            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Sample Job Title",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                Location = new Town { TownName = "Sample Town" },
                CompanyId = 111,
                CreatorId = Guid.NewGuid().ToString(),
                Company = company,
                MinSalary = 30000,
                MaxSalary = 50000,
                CreatedOn = DateTime.Now.AddDays(-7),
                IsDeleted = false,
                IsApproved = true,
            };

            await context.Companies.AddAsync(company);
            await context.Jobs.AddAsync(job);
            await context.SaveChangesAsync();

            await _companyService.HireUser("123", job.Id.ToString());

            var hiringRecord = await context.HiringRecords.FirstOrDefaultAsync();
            Assert.IsNotNull(hiringRecord);
            Assert.That(hiringRecord.JobId, Is.EqualTo(job.Id));
            Assert.That(hiringRecord.IsHired, Is.EqualTo(true));
        }

        [Test]
        public async Task IsHire_UserHired_ReturnsTrue()
        {
            // Test setup
            var userId = "user123";
            var jobId = Guid.NewGuid();

            var hiringRecord = new HiringRecord
            {
                CandidateId = userId,
                JobId = jobId,
                IsHired = true
            };

            await context.HiringRecords.AddAsync(hiringRecord);
            await context.SaveChangesAsync();

            // Test action
            var result = await _companyService.IsHire(userId, jobId.ToString());

            // Assert
            Assert.IsTrue(result.HasValue);
            Assert.IsTrue(result.Value);
        }

        [Test]
        public async Task IsHire_UserNotHired_ReturnsFalse()
        {
            // Test setup
            var userId = "user123";
            var jobId = Guid.NewGuid();

            var hiringRecord = new HiringRecord
            {
                JobId = jobId,
                CandidateId = userId,
                IsHired = false
            };

            await context.HiringRecords.AddAsync(hiringRecord);
            await context.SaveChangesAsync();

            // Test action
            var result = await _companyService.IsHire(userId, jobId.ToString());

            // Assert
            Assert.IsTrue(result.HasValue);
            Assert.IsFalse(result.Value);
        }

        [Test]
        public async Task IsHire_UserJobCombinationNotFound_ReturnsNull()
        {
            // Test setup
            var userId = "user123";
            var jobId = Guid.NewGuid();

            // Don't add any hiring records

            // Test action
            var result = await _companyService.IsHire(userId, jobId.ToString());

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task RejectUser_Succ()
        {
            var userId = "user123";
            var jobId = Guid.NewGuid().ToString();


            await _companyService.RejectUser(userId, jobId);


            var hiringRecord = await context.HiringRecords.FirstOrDefaultAsync(
                h => h.CandidateId == userId && h.JobId == Guid.Parse(jobId));


            Assert.IsNotNull(hiringRecord);
            Assert.IsFalse(hiringRecord.IsHired);
            Assert.That(hiringRecord.DateOfHiring.Date, Is.EqualTo(DateTime.Today));
        }

        [Test]
        public async Task GetUserIdByEmail_UserExists_ReturnsUserId()
        {
            // Test setup
            var userEmail = "test@example.com";
            var userId = "user123";

            var user = new ApplicationUser
            {
                Id = userId,
                Email = userEmail
            };

            await context.ApplicationUsers.AddAsync(user);
            await context.SaveChangesAsync();

            // Test action
            var resultUserId = await _companyService.GetUserIdByEmail(userEmail);

            // Assert
            Assert.That(resultUserId, Is.EqualTo(userId));
        }

        [Test]
        public async Task GetCompanyLogo_Succ()
        {
            var company = new Company
            {
                Id = 222,
                Name = "TestCompany",
                LogoUrl = "test_logo.png",
                ContactEmail = "koko@abc.bg",
                ContactPhone = "1234567890",
                UserId = "user123",
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();

            var logo = await _companyService.GetCompanyLogo(222);

            Assert.IsNotNull(logo);
            Assert.That(logo, Is.EqualTo("test_logo.png"));
        }
        [Test]
        public async Task GetCompanyLogo_With_Invalid_Id()
        {
            var company = new Company
            {
                Id = 222,
                Name = "TestCompany",
                LogoUrl = "test_logo.png",
                ContactEmail = "koko@abc.bg",
                ContactPhone = "1234567890",
                UserId = "user123",
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();

            var logo = await _companyService.GetCompanyLogo(1);

            Assert.IsNull(logo);
        }
    }
}