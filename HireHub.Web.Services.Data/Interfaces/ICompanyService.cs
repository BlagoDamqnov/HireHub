using HireHub.Web.ViewModels.Company;
using HireHub.Web.ViewModels.Jobs;

namespace HireHub.Web.Services.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICompanyService
    {
        Task CreateCompanyAsync(CreateCompanyVM createCompanyVM,string userId);
        Task<int> GetCompanyIdByUserId(string userId);
        Task<bool> IsUserHaveCompany(string userId);
        Task<string?> GetCompanyNameByUserId(string id);

        Task<ICollection<GetAllApplications>> MyApplication(int companyId);
    }
}
