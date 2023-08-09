using HireHub.Data;
using HireHub.Web.Services.Data;
using HireHub.Web.ViewModels.Resume;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace HireHub.Web.Tests
{
    public class ResumeServiceTest
    {
        private ApplicationDbContext context;
        private ResumeService _resumeService;

        [SetUp]
        public void SetUp()
        {
            var contextOption = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            this.context = new ApplicationDbContext(contextOption);
            this._resumeService = new ResumeService(context);
        }

        [TearDown]
        public void TearDown()
        {
            this.context.Database.EnsureDeleted();
        }

        [Test]
        public async Task AddResume_Succ()
        {
            var resume = new AddResumeVM
            {
                Name = "Test Resume",
                ResumePath = "Test Resume Path"
            };

            Assert.That(context.Resumes.Count(), Is.EqualTo(0));

            await _resumeService.AddResumeAsync(resume, Guid.NewGuid().ToString());

            Assert.That(context.Resumes.Count(), Is.EqualTo(1));

            var resumeInDb = context.Resumes.FirstOrDefault();

            Assert.That(resumeInDb!.Name, Is.EqualTo(resume.Name));
            Assert.That(resumeInDb.ResumePath, Is.EqualTo(resume.ResumePath));
        }
    }
}