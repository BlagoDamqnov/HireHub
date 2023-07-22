using HireHub.Web.ViewModels.Jobs;

namespace HireHub.Web.Services.Data.Models.House
{
    using System.Collections.Generic;

    public class AllJobsFilteredServiceModel
    {
        public IEnumerable<GetLastFiveJobsVM> Jobs { get; set; } = new List<GetLastFiveJobsVM>();
    }
}