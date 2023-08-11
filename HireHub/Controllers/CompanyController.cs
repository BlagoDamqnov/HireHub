using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Company;

namespace HireHub.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class CompanyController : UserController
    {
        private readonly ICompanyService _companyService;
        private readonly IEmailService _emailService;
        private readonly IJobService _jobService;
        public CompanyController(ICompanyService companyService, IEmailService emailService, IJobService jobService)
        {
            _companyService = companyService;
            _emailService = emailService;
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var isExist = await _companyService.IsUserHaveCompany(GetUserId());

            if (isExist)
            {
                return RedirectToAction("Edit", "Company");
            }
            else
            {
                if (User.IsInRole("Admin"))
                {
                    TempData["ErrorMessage"] = "Admin can't create a company!";
                    return RedirectToAction("Index", "Home");
                }
                TempData["InfoMessage"] = "You need to create a company first!";
                return View();
            }
        }

        [HttpGet]
        [Authorize(Policy = "CompanyOnly")]
        public async Task<IActionResult> Edit()
        {
            var model = await _companyService.GetCompanyByUserId(GetUserId());

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "CompanyOnly")]
        public async Task<IActionResult> Edit(EditCompanyVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid company data!";
                return View(model);
            }

            try
            {
                var editedCompany = await _companyService.EditCompanyAsync(model, GetUserId());
                TempData["SuccessMessage"] = "Company edited successfully!";
                return RedirectToAction("Edit");
            }
            catch (ArgumentException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Edit");
            }
        }

        [HttpPost]
        [Authorize(Policy = "WorkerOnly")]
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
        [Authorize(Policy = "CompanyOnly")]
        public async Task<IActionResult> MyApplication()
        {
            int companyId = await _companyService.GetCompanyIdByUserId(GetUserId());

            var myApplication = await _companyService.MyApplication(companyId);

            return View(myApplication);
        }

        [HttpPost]
        [Authorize(Policy = "CompanyOnly")]
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

        [Authorize(Policy = "CompanyOnly")]
        public async Task<IActionResult> Hire(string id, string email)
        {
            var getJob = await _jobService.GetJobDetails(id);
            string subject = $"Congratulations! You've been hired as a {getJob!.Title}";
            string message = $"Dear {email},<br/>" +
                $"Congratulations! You've been hired as a {getJob!.Title} at {getJob!.CompanyName}.<br/>" +
                $"Please contact us for more information.<br/>" +
                $"Best regards,<br/>" +
                $"HireHub Team";

            var userId = await _companyService.GetUserIdByEmail(email);
            var isApplicantHasCompany = await _companyService.IsUserHaveCompany(userId);
            if (isApplicantHasCompany)
            {
                TempData["ErrorMessage"] = "User already has a company!";
                return RedirectToAction("MyApplication", "Company");
            }
            var company = await _companyService.GetCompanyByUserId(GetUserId());

            bool? isHiring = await _companyService.IsHire(userId, id);

            if (isHiring == false)
            {
                TempData["ErrorMessage"] = "User is already rejected!";
                return RedirectToAction("MyApplication", "Company");
            }
            else if (isHiring == true)
            {
                TempData["ErrorMessage"] = "User is already hired!";
                return RedirectToAction("MyApplication", "Company");
            }

            await _emailService.SendEmailAsync(email, subject, message, "sandbox.smtp.mailtrap.io", 587, "0d230816b7e5c4", "6d99f5faff7358", $"{company.ContactEmail}");
            var applicantId = await _companyService.GetUserIdByEmail(email);

            await _companyService.HireUser(applicantId, id);
            TempData["SuccessMessage"] = "Email sent successfully!";
            return RedirectToAction("MyApplication", "Company");
        }

        [Authorize(Policy = "CompanyOnly")]
        public async Task<IActionResult> Reject(string id, string email)
        {
            var userId = await _companyService.GetUserIdByEmail(email);

            var getJob = await _jobService.GetJobDetails(id);
            string subject = $"We are sorry to inform you that you've been rejected as a {getJob!.Title}";
            string message = $"Dear {email},<br/>" +
                $"We are sorry to inform you that you've been rejected as a {getJob!.Title} at {getJob!.CompanyName}.<br/>" +
                $"Please contact us for more information.<br/>" +
                $"Best regards,<br/>" +
                $"HireHub Team";

            bool? isHiring = await _companyService.IsHire(userId, id);
            var company = await _companyService.GetCompanyByUserId(GetUserId());

            if (isHiring == false)
            {
                TempData["ErrorMessage"] = "User is already rejected!";
                return RedirectToAction("MyApplication", "Company");
            }
            else if (isHiring == true)
            {
                TempData["ErrorMessage"] = "User is already hired!";
                return RedirectToAction("MyApplication", "Company");
            }

            try
            {
                await _companyService.RejectUser(userId, id);
                await _emailService.SendEmailAsync(email, subject, message, "sandbox.smtp.mailtrap.io", 587, "0d230816b7e5c4", "6d99f5faff7358", $"{company.ContactEmail}");
            }
            catch (ArgumentException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("MyApplication", "Company");
            }

            TempData["SuccessMessage"] = "User rejected successfully!";
            return RedirectToAction("MyApplication", "Company");
        }
    }
}