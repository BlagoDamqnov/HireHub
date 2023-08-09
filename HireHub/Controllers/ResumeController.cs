using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Resume;

namespace HireHub.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ResumeController : UserController
    {
        private readonly ICompanyService _companyService;
        private readonly IResumeService _resumeService;

        public ResumeController(IResumeService resumeService, ICompanyService companyService)
        {
            _resumeService = resumeService;
            _companyService = companyService;
        }

        [HttpGet]
        [Authorize(Policy = "WorkerOnly")]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "WorkerOnly")]
        public async Task<IActionResult> Add(AddResumeVM resume)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
                TempData["ErrorMessage"] = error!.ErrorMessage;
                return RedirectToAction("Add", "Resume");
            }

            await _resumeService.AddResumeAsync(resume, GetUserId());
            TempData["SuccessMessage"] = "Resume added successfully";
            return RedirectToAction("Explore", "Job");
        }
    }
}