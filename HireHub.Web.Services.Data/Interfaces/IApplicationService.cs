using HireHub.Web.ViewModels.Application;

namespace HireHub.Web.Services.Data.Interfaces
{
    using HireHub.Web.ViewModels.Company;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IApplicationService
    {
        Task<ApplyForJobVM> GetMyResumesAsync(string userId, string jobId);

        Task AddApply(ApplyForJobVM model, string jobId, string userId);

        Task<IEnumerable<GetAllApplications>> GetMyApplication(string userId);

        Task RemoveApplication(string id, string userId);
    }
}