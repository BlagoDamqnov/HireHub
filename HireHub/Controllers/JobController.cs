﻿using HireHub.Web.Controllers;
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
        private readonly ICompanyService _companyService;
        public JobController(IJobService jobService, ICompanyService companyService)
        {
            _jobService = jobService;
            _companyService = companyService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Explore()
        {
            var jobs = await _jobService.GetLastFiveJobs();
            return View(jobs);
        }
        [HttpGet]
        public async Task<IActionResult> GetTownsByCountryId(int countryId)
        {
            var towns = await _jobService.GetTownsByCountryId(countryId);
            return Json(towns);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateJobVM model = await _jobService.GetNewJobAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateJobVM model)
        {
            var userId = GetUserId();
            var companyId = await _companyService.GetCompanyIdByUserId(userId);

            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "You have entered invalid data. Please try again.";
                return RedirectToAction("Create");
            }

            try
            {
                await _jobService.AddJobAsync(model, userId,companyId);
                TempData[SuccessMessage] = "You have successfully created a job offer.";
            }
            catch (System.Exception ex)
            {
                TempData[ErrorMessage] = $"{ex.Message}";
                return RedirectToAction("Create");
            }

            return RedirectToAction("Explore");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SearchJobs(string search)
        {
            var jobs = await _jobService.SearchJobs(search);
            return Json(jobs);
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

        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _jobService.DeleteJob(id);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error during delete a job!";
            }

            TempData[SuccessMessage] = "You have successfully deleted a job offer.";
            return RedirectToAction("Explore");
        }
    }
}
