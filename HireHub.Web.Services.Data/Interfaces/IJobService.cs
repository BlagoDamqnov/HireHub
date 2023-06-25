using HireHub.Web.ViewModels.Jobs;
using HireHub.Web.ViewModels.Towns;

namespace HireHub.Web.Services.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IJobService
    {
        public Task<IEnumerable<GetLastFiveJobsVM>> GetLastFiveJobs();
        public Task<CreateJobVM> GetNewJobAsync();
        Task<CreateJobVM> GetTownsByCountryId(CreateJobVM job,int countryId);
        Task AddJobAsync(CreateJobVM job, string creatorId);
        Task<IEnumerable<GetLastFiveJobsVM>> GetAllJobsForApprove();
        Task ApproveJob(Guid id);
        Task RejectJob(Guid id);
        Task<DetailsJobVM?> GetJobDetails(Guid id);
    }
}
