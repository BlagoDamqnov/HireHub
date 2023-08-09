using HireHub.Data;
using HireHub.Data.Entities;
using HireHub.Data.Models.Entities;
using HireHub.Web.Services.Data;
using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Jobs;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace HireHub.Web.Tests
{
    public class JobServiceTests
    {
        private ApplicationDbContext context;
        private JobService _jobService;

        [SetUp]
        public void SetUp()
        {
            var contextOption = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            this.context = new ApplicationDbContext(contextOption);
            var companyMoc = new Mock<ICompanyService>();
            this._jobService = new JobService(context, companyMoc.Object);
        }

        [TearDown]
        public void TearDown()
        {
            this.context.Database.EnsureDeleted();
        }

        [Test]
        public async Task GetJobsAsync_Succ()
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Sample Job Title",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                Location = new Town { TownName = "Sample Town" },
                CreatorId = Guid.NewGuid().ToString(),
                Company = new Company { Name = "Sample Company", LogoUrl = "sample_logo_url", ContactEmail = "test@abv.bg", UserId = Guid.NewGuid().ToString() },
                Category = new Category { CategoryName = "Sample Category" },
                MinSalary = 30000,
                MaxSalary = 50000,
                CreatedOn = DateTime.Now.AddDays(-7),
                IsDeleted = false,
                IsApproved = true,
            };

            context.Jobs.Add(job);
            await context.SaveChangesAsync();

            var queryModel = new AllJobsQueryModel
            {
            };
            var getJob = await _jobService.GetJobs(queryModel);
            var firstJob = getJob.Jobs.FirstOrDefault();

            Assert.That(getJob.Jobs.Count, Is.EqualTo(1));
            Assert.That(firstJob!.Id, Is.EqualTo(job.Id));
            Assert.That(firstJob.Title, Is.EqualTo(job.Title));
            Assert.IsNotNull(firstJob);
        }

        [Test]
        public async Task GetJobsAsync_WithSearchFilter()
        {
            var firstJob = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Sample Job Title",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                Location = new Town { TownName = "Sample Town" },
                CreatorId = Guid.NewGuid().ToString(),
                Company = new Company { Name = "Sample Company", LogoUrl = "sample_logo_url", ContactEmail = "test@abv.bg", UserId = Guid.NewGuid().ToString() },
                Category = new Category { CategoryName = "Sample Category" },
                MinSalary = 30000,
                MaxSalary = 50000,
                CreatedOn = DateTime.Now.AddDays(-7),
                IsDeleted = false,
                IsApproved = true,
            };
            var secondJob = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Second",
                Description = "Second Description",
                Requirements = "Second Requirements",
                Location = new Town { TownName = "Second Town" },
                CreatorId = Guid.NewGuid().ToString(),
                Company = new Company { Name = "Second Company", LogoUrl = "Second_url", ContactEmail = "testSecond@abv.bg", UserId = Guid.NewGuid().ToString() },
                Category = new Category { CategoryName = "Second Category" },
                MinSalary = 30000,
                MaxSalary = 50000,
                CreatedOn = DateTime.Now.AddDays(-7),
                IsDeleted = false,
                IsApproved = true,
            };
            context.Jobs.Add(firstJob);
            context.Jobs.Add(secondJob);
            await context.SaveChangesAsync();

            var queryModel = new AllJobsQueryModel
            {
                SearchString = "Second"
            };
            var otherQueryModel = new AllJobsQueryModel
            {
                SearchString = "Sample"
            };
            var getJob = await _jobService.GetJobs(queryModel);

            var emptyQueryModel = new AllJobsQueryModel();
            var withoutFilter = await _jobService.GetJobs(emptyQueryModel);

            var getOtherJob = await _jobService.GetJobs(otherQueryModel);

            Assert.That(getOtherJob.Jobs.Count, Is.EqualTo(1));
            Assert.That(getJob.Jobs.Count, Is.EqualTo(1));
            Assert.That(withoutFilter.Jobs.Count, Is.EqualTo(2));
            Assert.That(getJob.Jobs.FirstOrDefault()!.Title, Is.EqualTo(secondJob.Title));
        }

        [Test]
        public async Task GetTownsByCountryId_Succ()
        {
            int countryId = 1;
            var expectedTowns = new List<Town>
                {
                    new Town { Id = 1, CountryId = 1, TownName = "Town1" },
                    new Town { Id = 2, CountryId = 2, TownName = "Town2" }
                };

            context.Towns.AddRange(expectedTowns);
            await context.SaveChangesAsync();

            var towns = await _jobService.GetTownsByCountryId(countryId);
            var firstTown = await _jobService.GetTownsByCountryId(countryId);
            var invalidCountryId = await _jobService.GetTownsByCountryId(3);

            Assert.That(towns.Count(), Is.EqualTo(1));

            foreach (var town in towns)
            {
                Assert.That(town.Name, Is.EqualTo("Town1"));
                Assert.That(town.TownId, Is.EqualTo(1));
            }
            Assert.That(invalidCountryId.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task AddJob_Succ()
        {
            var job = new CreateJobVM
            {
                Title = "Added",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                TownId = 1,
                CategoryId = 1,
                MinSalary = 30000,
                MaxSalary = 50000,
            };

            await _jobService.AddJobAsync(job, Guid.NewGuid().ToString(), 1);

            Assert.IsTrue(context.Jobs.Any());

            var getJob = await context.Jobs.FirstOrDefaultAsync();

            Assert.That(getJob!.Title, Is.EqualTo(job.Title));
        }

        [Test]
        public Task AddJob_WhenMinSalaryIsBiggerThanMaxSalary()
        {
            var job = new CreateJobVM
            {
                Title = "Added",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                TownId = 1,
                CategoryId = 1,
                MinSalary = 300000,
                MaxSalary = 50000,
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _jobService.AddJobAsync(job, Guid.NewGuid().ToString(), 1));
            return Task.CompletedTask;
        }

        [Test]
        public async Task DeleteJob_Succ()
        {
            var userId = Guid.NewGuid().ToString();
            var job = new CreateJobVM
            {
                Title = "Added",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                TownId = 1,
                CategoryId = 1,
                MinSalary = 30000,
                MaxSalary = 50000
            };

            await _jobService.AddJobAsync(job, userId, 1);

            var getJob = await context.Jobs.FirstOrDefaultAsync();

            var jobId = getJob!.Id.ToString();

            Assert.IsTrue(context.Jobs.Any());
            Assert.IsFalse(getJob.IsDeleted);
            await _jobService.DeleteJob(jobId, userId);

            Assert.IsTrue(getJob.IsDeleted);
        }

        [Test]
        public async Task DeleteJob_Exception()
        {
            var userId = Guid.NewGuid().ToString();
            var job = new CreateJobVM
            {
                Title = "Added",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                TownId = 1,
                CategoryId = 1,
                MinSalary = 30000,
                MaxSalary = 50000
            };

            await _jobService.AddJobAsync(job, userId, 1);

            var getJob = await context.Jobs.FirstOrDefaultAsync();

            var jobId = getJob!.Id.ToString();

            Assert.IsTrue(context.Jobs.Any());
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _jobService.DeleteJob(jobId, Guid.NewGuid().ToString()));
        }

        [Test]
        public async Task GetAllJobsForApprove_Succ()
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Sample Job Title",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                Location = new Town { TownName = "Sample Town" },
                CreatorId = Guid.NewGuid().ToString(),
                Company = new Company { Name = "Sample Company", LogoUrl = "sample_logo_url", ContactEmail = "test@abv.bg", UserId = Guid.NewGuid().ToString(), IsDeleted = false },
                Category = new Category { CategoryName = "Sample Category" },
                MinSalary = 30000,
                MaxSalary = 50000,
                CreatedOn = DateTime.Now.AddDays(-7),
                IsDeleted = false,
                IsApproved = false,
            };

            var userId = Guid.NewGuid().ToString();

            await context.Jobs.AddAsync(job);
            await context.SaveChangesAsync();

            var getJob = await context.Jobs.FirstOrDefaultAsync();

            Assert.IsNotNull(getJob);
            Assert.IsFalse(getJob.IsApproved);
            Assert.IsFalse(getJob.IsDeleted);
            Assert.IsFalse(getJob.Company.IsDeleted);

            var jobs = await _jobService.GetAllJobsForApprove();

            Assert.IsNotNull(jobs);
            Assert.That(jobs.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllJobsForApprove_With_Deleted_Data()
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Sample Job Title",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                Location = new Town { TownName = "Sample Town" },
                CreatorId = Guid.NewGuid().ToString(),
                Company = new Company { Name = "Sample Company", LogoUrl = "sample_logo_url", ContactEmail = "test@abv.bg", UserId = Guid.NewGuid().ToString(), IsDeleted = true },
                Category = new Category { CategoryName = "Sample Category" },
                MinSalary = 30000,
                MaxSalary = 50000,
                CreatedOn = DateTime.Now.AddDays(-7),
                IsDeleted = false,
                IsApproved = false,
            };

            await context.Jobs.AddAsync(job);
            await context.SaveChangesAsync();

            var getJob = await context.Jobs.FirstOrDefaultAsync();

            Assert.IsNotNull(getJob);
            Assert.IsFalse(getJob.IsApproved);
            Assert.IsFalse(getJob.IsDeleted);
            Assert.IsTrue(getJob.Company.IsDeleted);

            var jobs = await _jobService.GetAllJobsForApprove();

            Assert.IsNotNull(jobs);
            Assert.That(jobs.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task ApproveJob_Succ()
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Sample Job Title",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                Location = new Town { TownName = "Sample Town" },
                CreatorId = Guid.NewGuid().ToString(),
                Company = new Company { Name = "Sample Company", LogoUrl = "sample_logo_url", ContactEmail = "test@abv.bg", UserId = Guid.NewGuid().ToString(), IsDeleted = true },
                Category = new Category { CategoryName = "Sample Category" },
                MinSalary = 30000,
                MaxSalary = 50000,
                CreatedOn = DateTime.Now.AddDays(-7),
                IsApproved = false,
            };

            await context.Jobs.AddAsync(job);
            await context.SaveChangesAsync();

            var jobId = job.Id.ToString();

            Assert.IsFalse(job.IsApproved);
            await _jobService.ApproveJob(jobId);

            Assert.IsTrue(job.IsApproved);
        }

        [Test]
        public async Task ApproveJob_With_Invalid_Job_Id()
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Sample Job Title",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                Location = new Town { TownName = "Sample Town" },
                CreatorId = Guid.NewGuid().ToString(),
                Company = new Company { Name = "Sample Company", LogoUrl = "sample_logo_url", ContactEmail = "test@abv.bg", UserId = Guid.NewGuid().ToString(), IsDeleted = true },
                Category = new Category { CategoryName = "Sample Category" },
                MinSalary = 30000,
                MaxSalary = 50000,
                CreatedOn = DateTime.Now.AddDays(-7),
                IsApproved = false,
            };

            await context.Jobs.AddAsync(job);
            await context.SaveChangesAsync();

            var jobId = job.Id.ToString();

            Assert.IsFalse(job.IsApproved);

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _jobService.ApproveJob(Guid.NewGuid().ToString()));
        }

        [Test]
        public async Task RejectJob_Succ()
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Sample Job Title",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                Location = new Town { TownName = "Sample Town" },
                CreatorId = Guid.NewGuid().ToString(),
                Company = new Company { Name = "Sample Company", LogoUrl = "sample_logo_url", ContactEmail = "test@abv.bg", UserId = Guid.NewGuid().ToString(), IsDeleted = true },
                Category = new Category { CategoryName = "Sample Category" },
                MinSalary = 30000,
                MaxSalary = 50000,
                CreatedOn = DateTime.Now.AddDays(-7),
                IsApproved = false,
                IsDeleted = false,
            };

            await context.Jobs.AddAsync(job);
            await context.SaveChangesAsync();

            Assert.IsFalse(job.IsDeleted);

            await _jobService.RejectJob(job.Id.ToString());

            Assert.IsTrue(job.IsDeleted);
        }

        [Test]
        public async Task RejectJob_With_Invalid_Job_Id()
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Sample Job Title",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                Location = new Town { TownName = "Sample Town" },
                CreatorId = Guid.NewGuid().ToString(),
                Company = new Company { Name = "Sample Company", LogoUrl = "sample_logo_url", ContactEmail = "test@abv.bg", UserId = Guid.NewGuid().ToString(), IsDeleted = true },
                Category = new Category { CategoryName = "Sample Category" },
                MinSalary = 30000,
                MaxSalary = 50000,
                CreatedOn = DateTime.Now.AddDays(-7),
                IsApproved = false,
                IsDeleted = false,
            };

            await context.Jobs.AddAsync(job);
            await context.SaveChangesAsync();

            Assert.IsFalse(job.IsDeleted);
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _jobService.RejectJob(Guid.NewGuid().ToString()));
        }

        [Test]
        public async Task GetJobDetails_Succ()
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Sample Job Title",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                Location = new Town { TownName = "Sample Town" },
                CreatorId = Guid.NewGuid().ToString(),
                Company = new Company { Name = "Sample Company", LogoUrl = "sample_logo_url", ContactEmail = "test@abv.bg", UserId = Guid.NewGuid().ToString(), IsDeleted = true },
                Category = new Category { CategoryName = "Sample Category" },
                MinSalary = 30000,
                MaxSalary = 50000,
                CreatedOn = DateTime.Now.AddDays(-7),
                IsApproved = false,
                IsDeleted = false,
            };

            await context.Jobs.AddAsync(job);
            await context.SaveChangesAsync();

            var jobs = await _jobService.GetJobDetails(job.Id.ToString());

            Assert.IsNotNull(jobs);
            Assert.That(jobs.Id, Is.EqualTo(job.Id));
            Assert.That(jobs.Title, Is.EqualTo(job.Title));
            Assert.That(jobs.Description, Is.EqualTo(job.Description));
            Assert.That(jobs.Requirements, Is.EqualTo(job.Requirements));
        }

        [Test]
        public async Task GetJobDetails_With_Invalid_Job_Id()
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Sample Job Title",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                Location = new Town { TownName = "Sample Town" },
                CreatorId = Guid.NewGuid().ToString(),
                Company = new Company { Name = "Sample Company", LogoUrl = "sample_logo_url", ContactEmail = "test@abv.bg", UserId = Guid.NewGuid().ToString(), IsDeleted = true },
                Category = new Category { CategoryName = "Sample Category" },
                MinSalary = 30000,
                MaxSalary = 50000,
                CreatedOn = DateTime.Now.AddDays(-7),
                IsApproved = false,
                IsDeleted = false,
            };

            await context.Jobs.AddAsync(job);
            await context.SaveChangesAsync();

            var jobDetails = await _jobService.GetJobDetails(Guid.NewGuid().ToString());

            Assert.IsNull(jobDetails);
        }

        [Test]
        public async Task GetJobDetailsForEdit_Succ()
        {
            var userId = Guid.NewGuid().ToString();

            var job = new CreateJobVM
            {
                Title = "Added",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                TownId = 1,
                CategoryId = 1,
                MinSalary = 30000,
                MaxSalary = 50000,
            };
            var category = new Category { Id = 1, CategoryName = "Sample Category" };
            var country = new Country { Id = 1, CountryName = "Sample Country" };
            var town = new Town { Id = 1, TownName = "Sample Town", Country = country };

            await context.Categories.AddAsync(category);
            await context.Countries.AddAsync(country);
            await context.Towns.AddAsync(town);
            await _jobService.AddJobAsync(job, userId, 1);

            await context.SaveChangesAsync();

            Assert.IsTrue(context.Jobs.Any(x => x.Title == "Added"));

            var jobId = await context.Jobs.FirstOrDefaultAsync();

            var jobDetails = await _jobService.GetJobDetailsForEdit(jobId!.Id.ToString(), userId);

            Assert.IsNotNull(jobDetails);
            Assert.That(jobDetails.Id, Is.EqualTo(jobId.Id));
            Assert.That(jobDetails.Title, Is.EqualTo(job.Title));
            Assert.That(jobDetails.Description, Is.EqualTo(job.Description));
        }

        [Test]
        public async Task GetJobDetailsForEdit_With_No_Owner_Id()
        {
            var userId = Guid.NewGuid().ToString();

            var job = new CreateJobVM
            {
                Title = "Added",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                TownId = 1,
                CategoryId = 1,
                MinSalary = 30000,
                MaxSalary = 50000,
            };

            await _jobService.AddJobAsync(job, Guid.NewGuid().ToString(), 1);

            await context.SaveChangesAsync();

            Assert.IsTrue(context.Jobs.Any(x => x.Title == "Added"));

            var jobId = await context.Jobs.FirstOrDefaultAsync();

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _jobService.GetJobDetailsForEdit(jobId!.Id.ToString(), userId));
        }

        [Test]
        public async Task GetJobDetailsForEdit_With_Invalid_Job_Id()
        {
            var userId = Guid.NewGuid().ToString();

            var jobDetails = await _jobService.GetJobDetailsForEdit(Guid.NewGuid().ToString(), userId);

            Assert.IsNull(jobDetails);
        }

        [Test]
        public async Task EditJob_Succ()
        {
            var userId = Guid.NewGuid().ToString();

            var job = new CreateJobVM
            {
                Title = "Added",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                TownId = 1,
                CategoryId = 1,
                MinSalary = 30000,
                MaxSalary = 50000,
            };
            var category = new Category { Id = 1, CategoryName = "Sample Category" };
            var country = new Country { Id = 1, CountryName = "Sample Country" };
            var town = new Town { Id = 1, TownName = "Sample Town", Country = country };

            await context.Categories.AddAsync(category);
            await context.Countries.AddAsync(country);
            await context.Towns.AddAsync(town);
            await _jobService.AddJobAsync(job, userId, 1);

            await context.SaveChangesAsync();

            var jobId = await context.Jobs.Select(x => x.Id).FirstOrDefaultAsync();

            var getJobForEdit = await _jobService.GetJobDetailsForEdit(jobId.ToString(), userId);

            var editJob = new EditJobVM
            {
                Id = getJobForEdit.Id,
                Title = "Edited",
                Description = "Edited Job Description",
                Requirements = "Edited Job Requirements",
                TownId = 1,
                CategoryId = 1,
                MinSalary = 30000,
                MaxSalary = 50000,
            };
            await _jobService.EditJob(editJob);
            var getEditedJob = await context.Jobs.FirstOrDefaultAsync();

            Assert.IsNotNull(getEditedJob);
            Assert.That(editJob.Id, Is.EqualTo(getEditedJob.Id));
            Assert.That(editJob.Title, Is.EqualTo(getEditedJob.Title));
            Assert.That(editJob.Description, Is.EqualTo(getEditedJob.Description));
        }

        [Test]
        public async Task EditJob_With_Invalid_Job_Id()
        {
            var userId = Guid.NewGuid().ToString();

            var job = new CreateJobVM
            {
                Title = "Added",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                TownId = 1,
                CategoryId = 1,
                MinSalary = 30000,
                MaxSalary = 50000,
            };
            await _jobService.AddJobAsync(job, userId, 1);

            var secondJob = new CreateJobVM
            {
                Title = "Added_Second",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                TownId = 1,
                CategoryId = 1,
                MinSalary = 30000,
                MaxSalary = 50000,
            };
            await _jobService.AddJobAsync(secondJob, userId, 1);

            var getSecondJob = await context.Jobs.FirstOrDefaultAsync(x => x.Title == "Added_Second");
            await _jobService.DeleteJob(getSecondJob!.Id.ToString(), userId);

            await context.SaveChangesAsync();
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _jobService.EditJob(new EditJobVM { Id = Guid.NewGuid() }));
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _jobService.EditJob(new EditJobVM { Id = getSecondJob!.Id }));
        }

        [Test]
        public async Task EditJob_With_Incorrect_Salary()
        {
            var userId = Guid.NewGuid().ToString();

            var job = new CreateJobVM
            {
                Title = "Added",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                TownId = 1,
                CategoryId = 1,
                MinSalary = 3000,
                MaxSalary = 50000,
            };
            var category = new Category { Id = 1, CategoryName = "Sample Category" };
            var country = new Country { Id = 1, CountryName = "Sample Country" };
            var town = new Town { Id = 1, TownName = "Sample Town", Country = country };

            await context.Categories.AddAsync(category);
            await context.Countries.AddAsync(country);
            await context.Towns.AddAsync(town);
            await _jobService.AddJobAsync(job, userId, 1);

            await context.SaveChangesAsync();

            var jobId = await context.Jobs.Select(x => x.Id).FirstOrDefaultAsync();

            var getJobForEdit = await _jobService.GetJobDetailsForEdit(jobId.ToString(), userId);

            var editJob = new EditJobVM
            {
                Id = getJobForEdit.Id,
                Title = "Edited",
                Description = "Edited Job Description",
                Requirements = "Edited Job Requirements",
                TownId = 1,
                CategoryId = 1,
                MinSalary = 30000,
                MaxSalary = 500,
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _jobService.EditJob(editJob));
        }

        [Test]
        public async Task GetJobsByCompanyId_Succ()
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Sample Job Title",
                Description = "Sample Job Description",
                Requirements = "Sample Job Requirements",
                Location = new Town { TownName = "Sample Town" },
                CreatorId = Guid.NewGuid().ToString(),
                Company = new Company { Id = 1, Name = "Sample Company", LogoUrl = "sample_logo_url", ContactEmail = "test@abv.bg", UserId = Guid.NewGuid().ToString() },
                Category = new Category { CategoryName = "Sample Category" },
                MinSalary = 30000,
                MaxSalary = 50000,
                CreatedOn = DateTime.Now.AddDays(-7),
                IsDeleted = false,
                IsApproved = true,
            };

            context.Jobs.Add(job);
            await context.SaveChangesAsync();

            var queryModel = new AllJobsQueryModel
            {
            };
            var getJob = await _jobService.GetJobs(queryModel);
            var firstJob = getJob.Jobs.FirstOrDefault();

            var getJobByCompanyId = await _jobService.GetJobsByCompanyId(1, queryModel);
            var zeroJobs = await _jobService.GetJobsByCompanyId(0, queryModel);
            Assert.That(getJobByCompanyId.Jobs.Count(), Is.EqualTo(1));
            Assert.That(zeroJobs.Jobs.Count(), Is.EqualTo(0));
        }
    }
}