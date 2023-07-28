using HireHub.Web.ViewModels.Jobs;
using HireHub.Web.ViewModels.Towns;

namespace HireHub.Web.Services.Data.Interfaces
{
    using HireHub.Web.Services.Data.Models.House;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IJobService
    {
        Task<AllJobsFilteredServiceModel> GetJobs(AllJobsQueryModel queryModel);

        public Task<CreateJobVM> GetNewJobAsync();

        Task<IEnumerable<TownVM>> GetTownsByCountryId(int countryId);

        Task AddJobAsync(CreateJobVM job, string creatorId, int companyId);

        Task<IEnumerable<GetJobsVM>> GetAllJobsForApprove();

        Task ApproveJob(string id);

        Task RejectJob(string id);

        Task<DetailsJobVM?> GetJobDetails(string id);

        Task DeleteJob(string id, string userId);

        Task<EditJobVM> GetJobDetailsForEdit(string id, string userId);

        Task EditJob(EditJobVM model);

        Task<AllJobsFilteredServiceModel> GetJobsByCompanyId(int companyId, AllJobsQueryModel queryModel);
    }
}