using HireHub.Web.ViewModels.Jobs.Enums;
using System.ComponentModel;

namespace HireHub.Web.ViewModels.Jobs
{
    using System.Collections.Generic;

    public class AllJobsQueryModel
    {
        public string? Category { get; set; }

        [DisplayName("Search")]
        public string? SearchString { get; set; }

        [DisplayName("Filter")]
        public JobSorting JobSorting { get; set; }

        public IEnumerable<string> Categories { get; set; } = new List<string>();

        public IEnumerable<GetJobsVM> Jobs { get; set; } = new List<GetJobsVM>();
    }
}