using HireHub.Web.ViewModels.Jobs;
using HireHub.Web.ViewModels.Towns;

namespace HireHub.Web.Services.Data.Interfaces
{
    using HireHub.Web.Services.Data.Models.House;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IJobService
    {
        Task<AllJobsFilteredServiceModel> GetLastFiveJobs(AllJobsQueryModel queryModel);
        public Task<CreateJobVM> GetNewJobAsync();
        Task<IEnumerable<TownVM>> GetTownsByCountryId(int countryId);
        Task AddJobAsync(CreateJobVM job, string creatorId,int companyId);
        Task<IEnumerable<GetLastFiveJobsVM>> GetAllJobsForApprove();
        Task ApproveJob(string id);
        Task RejectJob(string id);
        Task<DetailsJobVM?> GetJobDetails(string id);
        Task DeleteJob(string id);
        Task<IEnumerable<GetLastFiveJobsVM>> SearchJobs(string search);
    }
}
