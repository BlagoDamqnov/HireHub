namespace HireHub.Web.ViewModels.Jobs
{
    using HireHub.Data.Entities;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    // ReSharper disable once UnusedMember.Global
    public class DetailsJobVM
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CreatorId { get; set; } = null!;
        public string Title { get; set; } = null!;
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
