using HireHub.Controllers;
using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Company;

namespace HireHub.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class CompanyController : UserController
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var isExist = await _companyService.IsUserHaveCompany(GetUserId());

            if (isExist)
            {
                return RedirectToAction("IsExist", "Company");
            }
            return View();
        }
        public IActionResult IsExist()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyVM createCompanyVM)
        { 
            if (!ModelState.IsValid)
            {
                return View(createCompanyVM);
            }

            try
            {
                await _companyService.CreateCompanyAsync(createCompanyVM, GetUserId());
            }
            catch (ArgumentException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return View(createCompanyVM);
            }
           
            return RedirectToAction("Explore", "Job");
        }

        [HttpGet]
        public async Task<IActionResult> MyApplication()
        {
           int companyId = await _companyService.GetCompanyIdByUserId(GetUserId());

            var myApplication = await _companyService.MyApplication(companyId);

            return View(myApplication);
        }


    }
}
