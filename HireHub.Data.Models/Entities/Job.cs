using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HireHub.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Job
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CreatorId { get; set; } = null!;

        [ForeignKey(nameof(CreatorId))] 
        public IdentityUser User { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Requirements { get; set; } = null!;
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
        public int LocationId { get; set; }

        [ForeignKey(nameof(LocationId))]
        public Town Location { get; set; } = null!;
        public decimal MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LogoUrl { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
