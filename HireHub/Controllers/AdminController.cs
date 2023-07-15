using HireHub.Web.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HireHub.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IJobService _jobService;

        public AdminController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> AllJobsForApprove()
        {
            var jobs = await _jobService.GetAllJobsForApprove();
            return View(jobs);
        }
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveJob(string id)
        {
            try
            {
                await _jobService.ApproveJob(id);
            }
            catch
            {
                TempData["ErrorMessage"] = "Something went wrong!";
                return RedirectToAction("AllJobsForApprove");
            }

            TempData["SuccessMessage"] = "Job approved successfully!";
            return RedirectToAction("AllJobsForApprove");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectJob(string id)
        {
            try
            {
                await _jobService.RejectJob(id);
            }
            catch (InvalidOperationException)
            {
                TempData["ErrorMessage"] = "Something went wrong!";
                return RedirectToAction("AllJobsForApprove");
            }

            TempData["SuccessMessage"] = "Job rejected successfully!";
            return RedirectToAction("AllJobsForApprove");

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DetailsForAdmin(string id)
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
