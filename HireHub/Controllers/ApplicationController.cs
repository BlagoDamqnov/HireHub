using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Application;

namespace HireHub.Web.Controllers
{
    using HireHub.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
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
        [Authorize(Policy ="WorkerOnly")]
        public async Task<IActionResult> Apply(string id)
        {
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
        [Authorize(Policy = "WorkerOnly")]
        public async Task<IActionResult> Apply(ApplyForJobVM model, string id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Apply");
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

        [Authorize(Policy = "WorkerOnly")]
        public async Task<IActionResult> GetMyApplication()
        {
            if (!ModelState.IsValid)
            {
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

        [Authorize(Policy = "WorkerOnly")]
        public async Task<IActionResult> Remove(string id)
        {
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
