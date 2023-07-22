namespace HireHub.Web.ViewModels.Jobs
{
    using System;

    // ReSharper disable once UnusedMember.Global
    public class DetailsJobVM
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CreatorId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string Description { get; set; } = null!;
        public string Requirements { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string Location { get; set; } = null!;
        public decimal MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LogoUrl { get; set; } = null!;
    }
}