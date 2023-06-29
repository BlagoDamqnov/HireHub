using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HireHub.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace HireHub.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using static Common.EntityValidation.Job;
    public class Job
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CreatorId { get; set; } = null!;

        [ForeignKey(nameof(CreatorId))]
        public IdentityUser User { get; set; } = null!;

        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength,ErrorMessage= "Title must be between {2} and {1} length")]
        public string Title { get; set; } = null!;

        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength,ErrorMessage = "Description must be between {2} and {1} length")]
        public string Description { get; set; } = null!;

        [StringLength(RequirementsMaxLength, MinimumLength = RequirementsMinLength,ErrorMessage = "Requirements must be between {2} and {1} length")] 
        public string Requirements { get; set; } = null!;
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
        public int LocationId { get; set; }

        [ForeignKey(nameof(LocationId))]
        public Town Location { get; set; } = null!;
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; } = null!;

        [StringLength(MinSalaryMaxValue, MinimumLength = MinSalaryMinValue,ErrorMessage = "Min. Salary must be between {2} and {1} range")]
        public decimal MinSalary { get; set; }

        [StringLength(MaxSalaryMaxValue, MinimumLength = MaxSalaryMinValue,ErrorMessage = "Max. Salary must be between {2} and {1} range")]
        public decimal? MaxSalary { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LogoUrl { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
