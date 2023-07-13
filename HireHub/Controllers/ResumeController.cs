using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Resume;

namespace HireHub.Web.Controllers
{
    using HireHub.Web.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class ResumeController : UserController
    {
        private readonly IResumeService _resumeService;
        private readonly ICompanyService _companyService;
        public ResumeController(IResumeService resumeService, ICompanyService companyService)
        {
            _resumeService = resumeService;
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var isHaveCompany = await _companyService.IsUserHaveCompany(GetUserId());
            if (isHaveCompany)
            {
                TempData["ErrorMessage"] = "You can't add CV because you have a company";
                return RedirectToAction("Create", "Company");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddResumeVM resume)
        {
            var isHaveCompany = await _companyService.IsUserHaveCompany(GetUserId());
            if (isHaveCompany)
            {
                TempData["ErrorMessage"] = "You can't add CV because you have a company";
                return RedirectToAction("Create", "Company");
            }
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
                TempData["ErrorMessage"] = error!.ErrorMessage;
                return RedirectToAction("Add","Resume");
            }

            await _resumeService.AddResumeAsync(resume, GetUserId());
            TempData["SuccessMessage"] = "Resume added successfully";
            return RedirectToAction("Explore", "Job");
        }
    }
}
