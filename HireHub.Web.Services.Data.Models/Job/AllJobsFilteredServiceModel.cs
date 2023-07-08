using HireHub.Web.ViewModels.Jobs;

namespace HireHub.Web.Services.Data.Models.House
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AllJobsFilteredServiceModel
    {
        public IEnumerable<GetLastFiveJobsVM> Jobs { get; set; } = new List<GetLastFiveJobsVM>();
    }
}
