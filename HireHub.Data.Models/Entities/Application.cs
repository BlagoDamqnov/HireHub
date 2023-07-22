using HireHub.Data.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HireHub.Data.Entities
{
    using System;

    public class Application
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string ApplierId { get; set; } = null!;

        [ForeignKey(nameof(ApplierId))]
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public Guid JobId { get; set; }

        [ForeignKey(nameof(JobId))]
        public Job Job { get; set; } = null!;

        public int ResumeId { get; set; }

        [ForeignKey(nameof(ResumeId))]
        public Resume Resume { get; set; } = null!;

        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsApproved { get; set; } = false;
    }
}