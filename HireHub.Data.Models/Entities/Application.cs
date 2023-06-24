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

    public class Application
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ApplierId { get; set; } = null!;

        [ForeignKey(nameof(ApplierId))]
        public IdentityUser IdentityUser { get; set; } = null!;
        public Guid JobId { get; set; } 

        [ForeignKey(nameof(JobId))]
        public Job Job { get; set; } = null!;

        public int ResumeId { get; set; }

        [ForeignKey(nameof(ResumeId))]
        public Resume Resume { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
