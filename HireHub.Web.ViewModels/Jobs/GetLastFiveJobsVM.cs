// ReSharper disable All
namespace HireHub.Web.ViewModels.Jobs
{
    using System;

    public class GetLastFiveJobsVM
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? CreatorId { get; set; }
        public string Town { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public decimal MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LogoUrl { get; set; } = null!;
    }
}