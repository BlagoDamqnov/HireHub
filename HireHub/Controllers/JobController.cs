using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Jobs;

namespace HireHub.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    public class JobController : Controller
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        public async Task<IActionResult> Explore()
        {
            var jobs = await _jobService.GetLastFiveJobs();
            return View(jobs);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int countryId)
        {
            var model = await _jobService.GetNewJobAsync();

            var job = await _jobService.GetTownsByCountryId(model,1);

            return View(job);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateJobVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _jobService.AddJobAsync(model, userId);
            }
            catch (System.Exception)
            {
                return View(model);
            }
           
            return RedirectToAction("Explore");
        }
    }
}
