using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Resume;

namespace HireHub.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ResumeController : UserController
    {
        private readonly IResumeService _resumeService;

        public ResumeController(IResumeService resumeService)
        {
            _resumeService = resumeService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddResumeVM resume)
        {
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
