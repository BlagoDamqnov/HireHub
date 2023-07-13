using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Application;

namespace HireHub.Web.Controllers
{
    using HireHub.Controllers;
    using Microsoft.AspNetCore.Mvc;

    public class ApplicationController : UserController
    {
        private readonly IApplicationService _applicationService;
        private readonly ICompanyService _companyService;

        public ApplicationController(IApplicationService applicationService, ICompanyService companyService)
        {
            _applicationService = applicationService;
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> Apply(string id)
        {
            var userId = GetUserId();
            var isHaveCompany = await _companyService.IsUserHaveCompany(userId);
            if (isHaveCompany)
            {
                TempData["ErrorMessage"] = "You can't apply for a job because you have a company.";
                return RedirectToAction("Explore", "Job");
            }
            try
            {
                var resumes = await _applicationService.AddApplicationAsync(GetUserId(),id);
                return View(resumes);
            }
            catch (InvalidOperationException e)
            {
                TempData["ErrorMessage"] = e.Message;

                return RedirectToAction("Explore", "Job");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Apply(ApplyForJobVM model, string id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Apply");
            }
            var isHaveCompany = await _companyService.IsUserHaveCompany(GetUserId());
            if (isHaveCompany)
            {
                TempData["ErrorMessage"] = "You can't apply for a job because you have a company.";
                return RedirectToAction("Explore", "Job");
            }
            try
            {
                await _applicationService.AddApply(model, id, GetUserId());

                TempData["SuccessMessage"] = "You have successfully applied for this job.";
                return RedirectToAction("Explore", "Job");
            }
            catch (InvalidOperationException e)
            {
                TempData["ErrorMessage"] = e.Message;

                return RedirectToAction("Apply");
            }

        }

        public async Task<IActionResult> GetMyApplication()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Explore", "Job");
            }
            var isHaveCompany = await _companyService.IsUserHaveCompany(GetUserId());
            if (isHaveCompany)
            {
                TempData["ErrorMessage"] = "You can't view your application because you are company.";
                return RedirectToAction("Explore", "Job");
            }
            try
            {
                var myApplications = await _applicationService.GetMyApplication(GetUserId());
                return View(myApplications);
            }
            catch (InvalidOperationException)
            {
                TempData["ErrorMessage"] = "You don't have any applications.";

                return RedirectToAction("Explore", "Job");
            }
        }
        public async Task<IActionResult> Remove(string id)
        {
            var isHaveCompany = await _companyService.IsUserHaveCompany(GetUserId());
            if (isHaveCompany)
            {
                TempData["ErrorMessage"] = "You can't remove your application because you are company.";
                return RedirectToAction("Explore", "Job");
            }
            try
            {
                await _applicationService.RemoveApplication(id, GetUserId());

                TempData["SuccessMessage"] = "You have successfully removed your application.";
                return RedirectToAction("GetMyApplication");
            }
            catch (InvalidOperationException)
            {
                TempData["ErrorMessage"] = "You don't have any applications.";

                return RedirectToAction("GetMyApplication");
            }
        }

    }
}
