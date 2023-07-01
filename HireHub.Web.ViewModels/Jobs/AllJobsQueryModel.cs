using HireHub.Web.ViewModels.Jobs.Enums;

namespace HireHub.Web.ViewModels.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public class AllJobsQueryModel
    {
        public string? Category { get; set; }
        public string? SearchString { get; set; }
        public JobSorting JobSorting { get; set; }
        public IEnumerable<string> Categories { get; set; } = new List<string>();

        public IEnumerable<GetLastFiveJobsVM> Jobs { get; set; } = new List<GetLastFiveJobsVM>();

    }
}
