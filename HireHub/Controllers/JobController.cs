using HireHub.Web.Controllers;
using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.Services.Data.Models.House;
using HireHub.Web.ViewModels.Jobs;
using Microsoft.AspNetCore.Authorization;

namespace HireHub.Controllers
{
    using HireHub.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using System.Net.Mail;
    using System.Net;
    using System.Security.Claims;

    using static Common.NotificationMessagesConstants;

    [Authorize]
    public class JobController : UserController
    {
        private readonly IJobService _jobService;
        private readonly ICompanyService _companyService;
        private readonly ICategoryService _categoryService;
        public JobController(IJobService jobService, ICompanyService companyService, ICategoryService categoryService)
        {
            _jobService = jobService;
            _companyService = companyService;
            _categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Explore([FromQuery] AllJobsQueryModel queryModel)
        {
            AllJobsFilteredServiceModel jobs = await _jobService.GetJobs(queryModel);
            queryModel.Jobs = jobs.Jobs;

            queryModel.Categories = await _categoryService.GetAllCategoryNames();

            return View(queryModel);
        }
        [HttpGet]
        public async Task<IActionResult> GetTownsByCountryId(int countryId)
        {
            var towns = await _jobService.GetTownsByCountryId(countryId);
            return Json(towns);
        }

        [HttpGet]
        [Authorize(Policy = "CompanyOnly")]
        public async Task<IActionResult> Create()
        {
            CreateJobVM model = await _jobService.GetNewJobAsync();

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "CompanyOnly")]
        public async Task<IActionResult> Create(CreateJobVM model)
        {
            var isCategoryExist = await _categoryService.IsExist(model.CategoryId);

            if (isCategoryExist == false)
            {
                TempData[ErrorMessage] = "Category is invalid!";
                return RedirectToAction("Create");
            }

            var userId = GetUserId();
            var companyId = await _companyService.GetCompanyIdByUserId(userId);

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "You have entered invalid data. Please try again.";
                return RedirectToAction("Create");
            }

            try
            {
                await _jobService.AddJobAsync(model, userId, companyId);
                TempData[SuccessMessage] = "You have successfully created a job offer.";
            }
            catch (System.Exception ex)
            {
                TempData[ErrorMessage] = $"{ex.Message}";
                return RedirectToAction("Create");
            }

            return RedirectToAction("Explore");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _jobService.DeleteJob(id, GetUserId());
                TempData[SuccessMessage] = "You have successfully deleted a job offer.";

            }
            catch (InvalidOperationException e)
            {
                TempData[ErrorMessage] = e.Message;
            }
            return RedirectToAction("Explore");
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var job = await _jobService.GetJobDetailsForEdit(id, GetUserId());
                if (job == null)
                {
                    TempData[ErrorMessage] = "Error during edit a job!";
                    return RedirectToAction("Explore");
                }

                return View(job);
            }
            catch (InvalidOperationException e)
            {
                TempData[ErrorMessage] = e.Message;
            }
            return RedirectToAction("Explore");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditJobVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "Error during edit a job!";
                return RedirectToAction("Edit");
            }
            try
            {
                await _jobService.EditJob(model);
            }
            catch (InvalidOperationException)
            {
                TempData[ErrorMessage] = "Error during edit a job!";
            }

            return RedirectToAction("Explore");
        }
        public async Task<IActionResult> Details(string id)
        {
            var job = await _jobService.GetJobDetails(id);
            if (job != null)
            {
                return View(job);
            }
            return RedirectToAction("Explore");
        }
        [HttpGet]
        public async Task<IActionResult> GetCompanyJobs([FromQuery] AllJobsQueryModel queryModel)
        {
            var companyId = await _companyService.GetCompanyIdByUserId(GetUserId());
            AllJobsFilteredServiceModel jobs = await _jobService.GetJobsByCompanyId(companyId, queryModel);
            queryModel.Jobs = jobs.Jobs;

            queryModel.Categories = await _categoryService.GetAllCategoryNames();

            return View(queryModel);
        }
    }
}
