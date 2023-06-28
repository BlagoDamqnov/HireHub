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
                return RedirectToAction("Index","Home");
            }

            await _resumeService.AddResumeAsync(resume, GetUserId());
            TempData["SuccessMessage"] = "Resume added successfully";
            return RedirectToAction("Explore", "Job");
        }
    }
}
