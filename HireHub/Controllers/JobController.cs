using HireHub.Web.Controllers;
using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Jobs;
using Microsoft.AspNetCore.Authorization;

namespace HireHub.Controllers
{
    using HireHub.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using System.Security.Claims;

    using static Common.NotificationMessagesConstants;

    [Authorize]
    public class JobController : UserController
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Explore()
        {
            var jobs = await _jobService.GetLastFiveJobs();
            return View(jobs);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int countryId)
        {
            CreateJobVM model = await _jobService.GetNewJobAsync();

            var job = await _jobService.GetTownsByCountryId(model, 1);

            return View(job);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateJobVM model)
        {
            var userId = GetUserId();

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "You have entered invalid data. Please try again.";
                return RedirectToAction("Create");
            }

            try
            {
                await _jobService.AddJobAsync(model, userId);
                TempData[SuccessMessage] = "You have successfully created a job offer.";
            }
            catch (System.Exception)
            {
                return RedirectToAction("Create");
            }

            return RedirectToAction("Explore");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> AllJobsForApprove()
        {
            var jobs = await _jobService.GetAllJobsForApprove();
            return View(jobs);
        }

        public async Task<IActionResult> ApproveJob(Guid id)
        {
            try
            {
                await _jobService.ApproveJob(id);
            }
            catch
            {
                return RedirectToAction("AllJobsForApprove");
            }

            return RedirectToAction("AllJobsForApprove");
        }

        public async Task<IActionResult> RejectJob(Guid id)
        {
            try{
                await _jobService.RejectJob(id);
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("AllJobsForApprove");
            }
            
            return RedirectToAction("AllJobsForApprove");
            
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var job = await _jobService.GetJobDetails(id);
            if (job != null)
            {
                return View(job);
            }
           return RedirectToAction("Explore");
        }

        public async Task<IActionResult> DetailsForAdmin(Guid id)
        {
            var job = await _jobService.GetJobDetails(id);
            if (job != null)
            {
                return View(job);
            }
            return RedirectToAction("Explore");
        }
    }
}
