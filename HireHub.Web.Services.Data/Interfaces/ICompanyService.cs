using HireHub.Web.ViewModels.Company;

namespace HireHub.Web.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICompanyService
    {
        Task CreateCompanyAsync(CreateCompanyVM createCompanyVM, string userId);

        Task<int> GetCompanyIdByUserId(string userId);

        Task<bool> IsUserHaveCompany(string userId);

        Task<string?> GetCompanyNameByUserId(string id);

        Task<EditCompanyVM> EditCompanyAsync(EditCompanyVM editCompanyVM, string userId);

        Task<EditCompanyVM> GetCompanyByUserId(string userId);

        Task<bool> DeleteCompany(int id);

        Task<ICollection<GetAllApplications>> MyApplication(int companyId);

        Task HireUser(string userId, string jobId);

        Task<bool?> IsHire(string userId, string jobId);

        Task<string> GetUserIdByEmail(string? email);

        Task RejectUser(string userId, string jobId);

        Task<string> GetCompanyLogo(int companyId);
    }
}