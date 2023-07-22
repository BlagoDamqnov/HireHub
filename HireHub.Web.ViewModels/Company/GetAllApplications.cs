namespace HireHub.Web.ViewModels.Company
{
    using System;

    public class GetAllApplications
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string LogoUrl { get; set; } = null!;
        public string? Username { get; set; }
        public string Resume { get; set; } = null!;
    }
}