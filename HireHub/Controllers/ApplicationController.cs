using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Application;

namespace HireHub.Web.Controllers
{
    using HireHub.Controllers;
    using Microsoft.AspNetCore.Mvc;

    public class ApplicationController:UserController
    {
      private readonly IApplicationService _applicationService;

      public ApplicationController(IApplicationService applicationService)
      {
          _applicationService = applicationService;
      }

        [HttpGet]
        public async Task<IActionResult> Apply()
        {
            var resumes = await _applicationService.AddApplicationAsync(GetUserId());
            return View(resumes);
        }

        [HttpPost]
        public async Task<IActionResult> Apply(ApplyForJobVM model, Guid id)
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
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "You already applied for this job.";
                return RedirectToAction("Apply");
            }
           
        }
    }
}
