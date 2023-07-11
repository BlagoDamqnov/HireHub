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
                return RedirectToAction("Edit", "Company");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var model  = await _companyService.GetCompanyByUserId(GetUserId());

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCompanyVM model)
        {
            if(!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Enter invalid copmany data!";
                return View();
            }
            try
            {
                var editCompanyVM = await _companyService.EditCompanyAsync(model, GetUserId());
                TempData["SuccessMessage"] = "Company edited successfully!";
                return RedirectToAction("Edit", "Company");
            }
            catch (ArgumentException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Edit", "Company");
            }
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
                TempData["SuccessMessage"] = "Company created successfully!";
                return RedirectToAction("Explore", "Job");
            }
            catch (ArgumentException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return View(createCompanyVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> MyApplication()
        {
           int companyId = await _companyService.GetCompanyIdByUserId(GetUserId());

            var myApplication = await _companyService.MyApplication(companyId);

            return View(myApplication);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _companyService.DeleteCompany(id);
                TempData["SuccessMessage"] = "Company deleted successfully!";
                return RedirectToAction("Explore", "Job");
            }
            catch (ArgumentException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Edit", "Company");
            }
        }

    }
}
