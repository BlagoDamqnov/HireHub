using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HireHub.Data.Models.Entities;

using static HireHub.Common.EntityValidation.Resume;
namespace HireHub.Data.Entities
{
    public class Resume
    {
        public int Id { get; set; }

        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "Name must be between {2} and {1} length")]
        public string Name { get; set; } = null!;

        [StringLength(ResumePathMaxLength, MinimumLength = ResumePathMinLength, ErrorMessage = "Resume path must be between {2} and {1} length")]
        public string ResumePath { get; set; } = null!;
        public string CreatorId { get; set; } = null!;

        [ForeignKey(nameof(CreatorId))]
        public ApplicationUser IdentityUser { get; set; } = null!;

        public ICollection<Application> Applications { get; set; } = new List<Application>();

    }
}