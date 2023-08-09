using HireHub.Data;
using HireHub.Data.Entities;
using HireHub.Data.Models.Entities;
using HireHub.Web.Services.Data;
using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Jobs;
using HireHub.Web.ViewModels.Resume;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace HireHub.Web.Tests
{
    public class ApplicationServiceTest
    {
        private ApplicationDbContext context;
        private ApplicationService _applicationService;
        private JobService _jobService;
        private ResumeService _resumeService;

        [SetUp]
        public void SetUp()
        {
            var contextOption = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            this.context = new ApplicationDbContext(contextOption);
            this._applicationService = new ApplicationService(context);
            var companyMoc = new Mock<ICompanyService>();
            this._jobService = new JobService(context, companyMoc.Object);
            this._resumeService = new ResumeService(context);
        }

        [TearDown]
        public void TearDown()
        {
            this.context.Database.EnsureDeleted();
        }

        [Test]
        public async Task GetApplication_Succ()
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
            var getJob = await context.Jobs.FirstOrDefaultAsync(x => x.Title == "Added");

            var resume = new AddResumeVM
            {
                Name = "Test Resume",
                ResumePath = "Test Resume Path"
            };

            Assert.That(context.Resumes.Count(), Is.EqualTo(0));

            await _resumeService.AddResumeAsync(resume, userId);
            Assert.That(context.Resumes.Count(), Is.EqualTo(1));

            var application = await _applicationService.GetMyResumesAsync(userId, getJob!.Id.ToString());
            var myResume = application.Resumes.FirstOrDefault();
            Assert.IsNotNull(application);
            Assert.That(resume.Name, Is.EqualTo(myResume!.Name));
        }

        [Test]
        public async Task GetApplication_Owner_Error()
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
            var getJob = await context.Jobs.FirstOrDefaultAsync(x => x.Title == "Added");

            var resume = new AddResumeVM
            {
                Name = "Test Resume",
                ResumePath = "Test Resume Path"
            };

            Assert.That(context.Resumes.Count(), Is.EqualTo(0));

            await _resumeService.AddResumeAsync(resume, userId);
            Assert.That(context.Resumes.Count(), Is.EqualTo(1));

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _applicationService.GetMyResumesAsync(userId, getJob!.Id.ToString()));
        }

        [Test]
        public async Task AddApply_Succ()
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
            var getJob = await context.Jobs.FirstOrDefaultAsync(x => x.Title == "Added");

            var resume = new AddResumeVM
            {
                Name = "Test Resume",
                ResumePath = "Test Resume Path"
            };

            Assert.That(context.Resumes.Count(), Is.EqualTo(0));

            await _resumeService.AddResumeAsync(resume, userId);
            Assert.That(context.Resumes.Count(), Is.EqualTo(1));

            var model = await _applicationService.GetMyResumesAsync(userId, getJob!.Id.ToString());

            Assert.That(context.Applications.Count(), Is.EqualTo(0));
            await _applicationService.AddApply(model, getJob!.Id.ToString(), userId);
            Assert.That(context.Applications.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task AddApply_Already_Apply_Error()
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
            var getJob = await context.Jobs.FirstOrDefaultAsync(x => x.Title == "Added");

            var resume = new AddResumeVM
            {
                Name = "Test Resume",
                ResumePath = "Test Resume Path"
            };

            Assert.That(context.Resumes.Count(), Is.EqualTo(0));

            await _resumeService.AddResumeAsync(resume, userId);
            Assert.That(context.Resumes.Count(), Is.EqualTo(1));

            var model = await _applicationService.GetMyResumesAsync(userId, getJob!.Id.ToString());

            Assert.That(context.Applications.Count(), Is.EqualTo(0));
            await _applicationService.AddApply(model, getJob!.Id.ToString(), userId);
            Assert.That(context.Applications.Count(), Is.EqualTo(1));

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _applicationService.AddApply(model, getJob!.Id.ToString(), userId));
        }
        [Test]
        public async Task RemoveApplication_ApplicationExists_RemovesApplication()
        {
            // Arrange
            var userId = "user123";
            var jobId = Guid.NewGuid();

            var application = new Application
            {
                ApplierId = userId,
                JobId = jobId,
                IsDeleted = false
            };
            context.Applications.Add(application);
            context.SaveChanges();

            // Act
            await _applicationService.RemoveApplication(jobId.ToString(), userId);

            //Assert
            Assert.IsTrue(application.IsDeleted);
        }

        [Test]
        public async Task RemoveApplication_ApplicationDoesNotExist_DoesNothing()
        {
            // Arrange
            var userId = "user123";
            var jobId = Guid.NewGuid();

            // Act
            await _applicationService.RemoveApplication(jobId.ToString(), userId);

            // Assert
            var removedApplication = context.Applications
                .FirstOrDefault(x => x.ApplierId == userId && x.JobId == jobId && x.IsDeleted == true);
            Assert.IsNull(removedApplication);
        }
    }
}
