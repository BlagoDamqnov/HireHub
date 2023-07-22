using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireHub.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IJobService _jobService;
        private readonly ICategoryService _categoryService;

        public AdminController(IJobService jobService, ICategoryService categoryService)
        {
            _jobService = jobService;
            _categoryService = categoryService;
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
            return RedirectToAction("Explore", "Job");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCategories()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategories(CreateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Something went wrong!";

                return RedirectToAction("CreateCategories");
            }

            try
            {
                await _categoryService.Create(model);
            }
            catch (InvalidOperationException e)
            {
                TempData["ErrorMessage"] = e.Message;
                StatusCode(400);
                return RedirectToAction("CreateCategories");
            }
            TempData["SuccessMessage"] = "Category created successfully!";
            return RedirectToAction("CreateCategories");
        }
    }
}