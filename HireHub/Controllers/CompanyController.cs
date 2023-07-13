﻿using HireHub.Controllers;
using HireHub.Web.Services.Data.Interfaces;
using HireHub.Web.ViewModels.Company;

namespace HireHub.Web.Controllers
{
    using HireHub.Web.Services.Data;
    using Microsoft.AspNetCore.Mvc;
    using System.Text.Encodings.Web;

    public class CompanyController : UserController
    {
        private readonly ICompanyService _companyService;
        private readonly IEmailService _emailService;
        private readonly IJobService _jobService;
        public CompanyController(ICompanyService companyService, IEmailService emailService,IJobService jobService)
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
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var model = await _companyService.GetCompanyByUserId(GetUserId());

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCompanyVM model)
        {
            if (!ModelState.IsValid)
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

        public async Task<IActionResult> Hire(string id,string email)
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
            var company= await _companyService.GetCompanyByUserId(GetUserId());


            bool? isHiring = await _companyService.IsHire(userId, id);


            if (isHiring==false)
            {
                TempData["ErrorMessage"] = "User is already rejected!";
                return RedirectToAction("MyApplication", "Company");
            }else if(isHiring == true)
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

        public async Task<IActionResult> Reject(string id , string email)
        {
            var userId = await _companyService.GetUserIdByEmail(email);

            bool? isHiring = await _companyService.IsHire(userId, id);

            if(isHiring == false)
            {
                TempData["ErrorMessage"] = "User is already rejected!";
                return RedirectToAction("MyApplication", "Company");
            }else if (isHiring == true)
            {
                TempData["ErrorMessage"] = "User is already hired!";
                return RedirectToAction("MyApplication", "Company");
            }

            await _companyService.RejectUser(userId, id);
            TempData["SuccessMessage"] = "User rejected successfully!";
            return RedirectToAction("MyApplication", "Company");
        }
    }
}
